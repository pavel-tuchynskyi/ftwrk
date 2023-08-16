using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Domain.Entities.Songs;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Common.Models;
using FTWRK.Persistance.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistSongsStrategy
{
    public class GetPlaylistSongsQueryParams : IPlaylistSongs
    {
        private readonly IMongoContext _dbContext;

        public GetPlaylistSongsQueryParams(IMongoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId)
        {
            Log.Debug("{method} is started in stategy {stategy}", nameof(GetAll), nameof(GetPlaylistSongsQueryParams));

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

            var songsDto = await collection
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
                    { nameof(SongDto.AlbumTitle), $"${nameof(AlbumSongs)}.{nameof(Album.Title)}" },
                    { nameof(SongDto.AlbumId), $"${nameof(AlbumSongs)}._id" },
                    { "_id", 0 }
                })
                .Unwind(nameof(Song))
                .AppendStage<BsonDocument>(favoritePlaylist)
                .AppendStage<BsonDocument>(isFavorite)
                .Project(new BsonDocument
                {
                    { "_id", $"${nameof(Song)}._id" },
                    { nameof(SongDto.Artists), $"${nameof(Song)}.{nameof(Song.Artists)}" },
                    { nameof(SongDto.Title), $"${nameof(Song)}.{nameof(Song.Title)}" },
                    { nameof(SongDto.Duration), $"${nameof(Song)}.{nameof(Song.Duration)}" },
                    { nameof(SongDto.IsArchived), $"${nameof(Song)}.{nameof(Song.IsArchived)}" },
                    { nameof(SongDto.AlbumId), $"${nameof(SongDto.AlbumId)}" },
                    { nameof(SongDto.IsFavorite), 1 },
                    { nameof(SongDto.AlbumTitle), $"${nameof(SongDto.AlbumTitle)}" }
                })
                .As<SongDto>()
                .OrderBy(parameters.OrderBy)
                .ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            Log.Debug("{method} is finished successfully in strategy {servstrategyice}", nameof(GetAll), nameof(GetPlaylistSongsQueryParams));
            return songsDto;
        }
    }
}
