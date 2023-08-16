using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Domain.Entities.Songs;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistSongsStrategy
{
    public class GetPlaylistSongsLinkedList : IPlaylistSongs
    {
        private readonly IMongoContext _dbContext;

        public GetPlaylistSongsLinkedList(IMongoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId)
        {
            Log.Debug("{method} is started in stategy {stategy}", nameof(GetAll), nameof(GetPlaylistSongsLinkedList));

            var collection = _dbContext.GetCollection<CustomPlaylist>();
            var albumCollectionName = _dbContext.GetCollectionName<Album>();
            var playlistCollectionName = _dbContext.GetCollectionName<CustomPlaylist>();

            BsonBinaryData userIdBinary = new BsonBinaryData(userId, GuidRepresentation.Standard);
            BsonArray pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$match", new BsonDocument
            {
                { "$expr", new BsonDocument
                {
                    { "$and", new BsonArray{
                        new BsonDocument{ { "$eq", new BsonArray { $"${nameof(CustomPlaylist.OwnerId)}", userIdBinary } } },
                        new BsonDocument{ { "$eq", new BsonArray { $"${nameof(CustomPlaylist.IsCustom)}", false } } }
                    } }
                }
                }
            }));

            pipeline.Add(new BsonDocument("$unwind", new BsonDocument("path", $"${nameof(CustomPlaylist.Songs)}")));

            pipeline.Add(new BsonDocument("$replaceRoot", new BsonDocument
            {
                { "newRoot", $"${nameof(CustomPlaylist.Songs)}" }
            }));

            var favoritePlaylist = new BsonDocument("$lookup", new BsonDocument("from", playlistCollectionName)
                .Add("pipeline", pipeline)
                .Add("as", "Favorites")
            );

            var isFavorite = new BsonDocument("$addFields", new BsonDocument
            {
                { "IsFavorite", new BsonDocument
                {
                    { "$in", new BsonArray{ $"${nameof(Song)}._id", $"$Favorites.{nameof(PlaylistSong.SongId)}"} }
                } }
            });

            var songsResult = await collection
                .Aggregate()
                .Match(x => x.Id == documentId)
                .Unwind(x => x.Songs)
                .ReplaceRoot(x => x[nameof(CustomPlaylist.Songs)])
                .Lookup(albumCollectionName, nameof(PlaylistSong.AlbumId), "_id", nameof(AlbumSongs))
                .Unwind(nameof(AlbumSongs))
                .Project(new BsonDocument
                {
                    { nameof(Song), new BsonDocument{
                        { "$filter", new BsonDocument{
                            { "input", $"${nameof(AlbumSongs)}.{nameof(AlbumSongs.Songs)}" },
                            { "as", "item" },
                            { "cond", new BsonDocument{
                                { "$eq", new BsonArray(){ "$$item._id", $"${nameof(PlaylistSong.SongId)}" } }
                            } }
                        } }
                    } },
                    { nameof(PlaylistSong.Previous), $"${nameof(PlaylistSong.Previous)}" },
                    { nameof(PlaylistSong.Next), $"${nameof(PlaylistSong.Next)}" },
                    { nameof(SongDto.AlbumTitle), $"${nameof(AlbumSongs)}.{nameof(Album.Title)}" },
                    { nameof(SongDto.AlbumId), $"${nameof(AlbumSongs)}._id" }
                })
                .Unwind(nameof(Song))
                .AppendStage<BsonDocument>(favoritePlaylist)
                .AppendStage<BsonDocument>(isFavorite)
                .Project(new BsonDocument
                {
                    { nameof(PlaylistSong.Previous), $"${nameof(PlaylistSong.Previous)}" },
                    { nameof(PlaylistSong.Next), $"${nameof(PlaylistSong.Next)}" },
                    { nameof(PlaylistSongResult.Song), new BsonDocument
                    {
                        { "_id", $"${nameof(Song)}._id" },
                        { nameof(Song.Artists), $"${nameof(Song)}.{nameof(Song.Artists)}" },
                        { nameof(Song.Title), $"${nameof(Song)}.{nameof(Song.Title)}" },
                        { nameof(Song.Duration), $"${nameof(Song)}.{nameof(Song.Duration)}" },
                        { nameof(Song.IsArchived), $"${nameof(Song)}.{nameof(Song.IsArchived)}" },
                        { nameof(SongDto.AlbumId), $"${nameof(SongDto.AlbumId)}" },
                        { nameof(SongDto.AlbumTitle), $"${nameof(SongDto.AlbumTitle)}" },
                        { nameof(SongDto.IsFavorite), $"${nameof(SongDto.IsFavorite)}" }
                    } }
                })
                .As<PlaylistSongResult>()
                .ToListAsync();

            var sortedSongs = SortByHierarchy(songsResult);
            var songDto = new PagedList<SongDto>(sortedSongs, sortedSongs.Count(), parameters.PageNumber, parameters.PageSize);

            Log.Debug("{method} is finished successfully in strategy {servstrategyice}", nameof(GetAll), nameof(GetPlaylistSongsLinkedList));
            return songDto;
        }

        private IEnumerable<SongDto> SortByHierarchy(List<PlaylistSongResult> playlistSongs)
        {
            if (playlistSongs.Count == 0)
            {
                yield break;
            }

            var songDictionary = playlistSongs.ToDictionary
            (
                entity => entity.Previous ?? new Guid(),
                entity => entity
            );

            var key = new Guid();
            PlaylistSongResult songResult;

            do
            {
                songResult = songDictionary[key];
                yield return songResult.Song;
                key = songResult.Id;
            }
            while (songResult.Next != null);
        }
    }
}
