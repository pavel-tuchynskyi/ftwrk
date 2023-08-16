using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Common;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Domain.Entities.Songs;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services
{
    public class AlbumSongService : ISongService
    {
        private readonly IMongoCollection<Album> _collection;
        private readonly IMongoContext _dbContext;
        private readonly ISongBlobService _songBlobService;

        public AlbumSongService(IMongoContext dbContext, ISongBlobService songBlobService)
        {
            _collection = dbContext.GetCollection<Album>();
            _dbContext = dbContext;
            _songBlobService = songBlobService;
        }

        public async Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId)
        {
            Log.Debug("{method} is started in {service}", nameof(GetAll), nameof(AlbumSongService));

            var playlistCollectionName = _dbContext.GetCollectionName<CustomPlaylist>();
            FilterDefinition<Album> filter = null;

            if(documentId != null)
            {
                filter = Builders<Album>.Filter.Eq(x => x.Id, documentId);
            }
            else
            {
                filter = Builders<Album>.Filter.Empty;
            }
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
                    { "$in", new BsonArray{ $"${nameof(Album.Songs)}._id", $"$Favorites.{nameof(PlaylistSong.SongId)}"} }
                } }
            });

            var songs = await _collection.Aggregate()
                .Match(filter)
                .Unwind(x => x.Songs)
                .AppendStage<BsonDocument>(favoritePlaylist)
                .AppendStage<BsonDocument>(isFavorite)
                .Project(new BsonDocument
                {
                    { "_id", $"${nameof(Album.Songs)}._id" },
                    { nameof(SongDto.Title), $"${nameof(Album.Songs)}.{nameof(Song.Title)}" },
                    { nameof(SongDto.Artists), $"${nameof(Album.Songs)}.{nameof(Song.Artists)}" },
                    { nameof(SongDto.Duration), $"${nameof(Album.Songs)}.{nameof(Song.Duration)}" },
                    { nameof(SongDto.IsArchived), $"${nameof(Album.Songs)}.{nameof(Song.IsArchived)}" },
                    { nameof(SongDto.AlbumTitle), $"${nameof(Album.Title)}" },
                    { nameof(SongDto.IsFavorite), 1 },
                    { nameof(SongDto.AlbumId), "$_id"}
                })
                .As<SongDto>()
                .Filter(parameters.Filter)
                .OrderBy(parameters.OrderBy)
                .ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            Log.Debug("{method} is finished successfully in {service}", nameof(GetAll), nameof(AlbumSongService));

            return songs;
        }

        public async Task<SongBase> Get(Guid documentId, Guid? id)
        {
            Log.Debug("{method} is started in {service}", nameof(Get), nameof(AlbumSongService));

            var song = await _collection
                .AsQueryable()
                .Where(x => x.Id == documentId)
                .SelectMany(x => x.Songs)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (song == null)
            {
                Log.Error("Can't find song with id: {id}", id);
                throw new NotFoundException("Can't find this song");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Get), nameof(AlbumSongService));

            return song;
        }

        public async Task<bool> Add(Guid documentId, Guid creatorId, SongBase song)
        {
            Log.Debug("{method} is started in {service}", nameof(Add), nameof(AlbumSongService));

            var albumSong = (AlbumSong)song;
            var update = Builders<Album>.Update.AddToSet(x => x.Songs, albumSong.Song);

            using(var session = await _dbContext.StartSessionAsync())
            {
                Log.Information("Starting transaction");
                session.StartTransaction();

                try
                {
                    var dbSongAdd = await _collection.UpdateOneAsync(x => x.Id == documentId && x.CreatorId == creatorId, update);
                    var storageSongAdd = await _songBlobService.Upload(albumSong.SongBlob);

                    await session.CommitTransactionAsync();

                    Log.Information("Transaction is completed");
                    Log.Debug("{method} is finished successfully in {service}", nameof(Add), nameof(AlbumSongService));

                    return true;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    await _songBlobService.Delete(albumSong.Song.Id);
                    Log.Error("Can't add song to album with id: {id}. Transaction is aborted", documentId);
                    return false;
                }
            }
        }

        public async Task<bool> Update(Guid documentId, Guid creatorId, SongBase song)
        {
            Log.Debug("{method} is started in {service}", nameof(Update), nameof(AlbumSongService));

            var songUpdate = song as Song;
            var filter = Builders<Album>.Filter;
            var songFilter = Builders<Album>.Filter.And(
                filter.Eq(x => x.Id, documentId),
                filter.Eq(x => x.CreatorId, creatorId),
                filter.ElemMatch(x => x.Songs, x => x.Id == songUpdate.Id));

            var update = Builders<Album>.Update
                .Set(x => x.Songs[-1].Artists, songUpdate.Artists)
                .Set(x => x.Songs[-1].Title, songUpdate.Title);

            var result = await _collection.UpdateOneAsync(songFilter, update);

            if (result.ModifiedCount == 0)
            {
                Log.Error("Can't update song with id: {id} in album with id: {id}", songUpdate.Id, documentId);
                throw new ApplicationException("Can't update this song");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Update), nameof(AlbumSongService));

            return true;
        }

        public async Task<bool> Delete(Guid documentId, Guid creatorId, Guid id)
        {
            Log.Debug("{method} is started in {service}", nameof(Delete), nameof(AlbumSongService));

            var filter = Builders<Album>.Filter;
            var songFilter = Builders<Album>.Filter.And(
                filter.Eq(x => x.Id, documentId),
                filter.Eq(x => x.CreatorId, creatorId),
                filter.ElemMatch(x => x.Songs, x => x.Id == id));

            var update = Builders<Album>.Update
                .Set(x => x.Songs[-1].IsArchived, true);

            var songBlob = await _songBlobService.Get(id);

            using(var session = await _dbContext.StartSessionAsync())
            {
                Log.Information("Starting transaction");
                session.StartTransaction();

                try
                {
                    var dbDeleteSong = _collection.UpdateOneAsync(songFilter, update);
                    var storageDeleteSong = _songBlobService.Delete(id);

                    await Task.WhenAll(dbDeleteSong, storageDeleteSong);
                    await session.CommitTransactionAsync();

                    Log.Information("Transaction is completed");
                    Log.Debug("{method} is finished successfully in {service}", nameof(Delete), nameof(AlbumSongService));
                    return true;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();

                    if(!await _songBlobService.IsExist(id))
                    {
                        await _songBlobService.Upload(songBlob);
                    }

                    Log.Error("Can't delete song with id: {id} in album with id: {id}. Transaction is aborted", id, documentId);
                    throw new ApplicationException("Can't delete this song");
                }
            }
        }
    }
}
