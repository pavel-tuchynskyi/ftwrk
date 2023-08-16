using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Common;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Domain.Entities.Songs;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistSongsStrategy;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog;
using System.Linq.Expressions;

namespace FTWRK.Persistance.Mongo.Services
{
    public class PlaylistSongService : ISongService
    {
        private readonly IMongoCollection<CustomPlaylist> _collection;
        private readonly IMongoContext _dbContext;

        public PlaylistSongService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<CustomPlaylist>();
            _dbContext = dbContext;
        }

        public async Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId)
        {
            Log.Debug("{method} is started in {service}", nameof(GetAll), nameof(PlaylistSongService));

            var strategy = new GetPlaylistSongsResolver(_dbContext).GetStrategy(parameters);

            var songs = await strategy.GetAll(parameters, userId, documentId);

            Log.Debug("{method} is finished successfully in {service}", nameof(GetAll), nameof(PlaylistSongService));
            return songs;
        }

        public async Task<SongBase> Get(Guid documentId, Guid? id)
        {
            Log.Debug("{method} is started in {service}", nameof(Get), nameof(PlaylistSongService));

            var song = await _collection
                .AsQueryable()
                .Where(x => x.Id == documentId)
                .SelectMany(x => x.Songs)
                .Where(x => x.SongId == id)
                .FirstOrDefaultAsync();

            Log.Debug("{method} is finished successfully in {service}", nameof(Get), nameof(PlaylistSongService));
            return song;
        }

        public async Task<bool> Add(Guid documentId, Guid creatorId, SongBase song)
        {
            Log.Debug("{method} is started in {service}", nameof(Add), nameof(PlaylistSongService));
            var playlistSong = (PlaylistSong)song;

            if (await IsSongInPlaylist(documentId, playlistSong.SongId))
            {
                Log.Error("Song: {songId} is already in playlist: {id}", playlistSong.SongId, documentId);
                throw new BadRequestException("Song already in playlist");
            }

            var lastSong = await _collection
                .AsQueryable()
                .Where(x => x.Id == documentId && x.OwnerId == creatorId)
                .SelectMany(x => x.Songs)
                .Where(x => x.Next == null)
                .FirstOrDefaultAsync();

            playlistSong.Previous = lastSong != null ? lastSong.Id : null;

            var builder = Builders<CustomPlaylist>.Filter;
            var songFilter = builder.And(builder.Eq(x => x.Id, documentId), builder.Eq(x => x.OwnerId,creatorId));
            var update = Builders<CustomPlaylist>.Update.AddToSet(x => x.Songs, playlistSong);

            using(var session = await _dbContext.StartSessionAsync())
            {
                Log.Information("Starting transaction");
                session.StartTransaction();

                try
                {
                    var requests = new List<WriteModel<CustomPlaylist>>();
                    requests.Add(new UpdateOneModel<CustomPlaylist>(songFilter, update));

                    if (lastSong != null)
                    {
                        requests.Add(UpdateSongPosition(documentId, x => x.Next == null, x => x.Songs[-1].Next, playlistSong.Id));
                    }

                    await _collection.BulkWriteAsync(requests);

                    await session.CommitTransactionAsync();

                    Log.Information("Transaction is completed");
                    Log.Debug("{method} is finished successfully in {service}", nameof(Add), nameof(PlaylistSongService));
                    return true;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    Log.Error("Can't add song: {songId} in playlist: {id}. Transaction is aborted", playlistSong.SongId, documentId);
                    throw new ApplicationException("Can't add this song");
                }
            }
        }

        private async Task<bool> IsSongInPlaylist(Guid documentId, Guid songId)
        {
            var isExist = await _collection
                .AsQueryable()
                .Where(x => x.Id == documentId)
                .SelectMany(x => x.Songs)
                .AnyAsync(x => x.SongId == songId);

            return isExist;
        }

        public async Task<bool> Update(Guid documentId, Guid creatorId, SongBase song)
        {
            Log.Debug("{method} is started in {service}", nameof(Update), nameof(PlaylistSongService));
            var playlistSong = (PlaylistSong)song;

            var songs = await _collection.AsQueryable()
                .Where(x => x.Id == documentId)
                .SelectMany(x => x.Songs)
                .Where(x => x.SongId == playlistSong.SongId || x.SongId == playlistSong.Previous || x.SongId == playlistSong.Next)
                .ToListAsync();

            var songToUpdate = songs.FirstOrDefault(x => x.SongId == playlistSong.SongId);
            var newPrevious = songs.FirstOrDefault(x => x.SongId == playlistSong.Previous);
            var newNext = songs.FirstOrDefault(x => x.SongId == playlistSong.Next);

            using (var session = await _dbContext.StartSessionAsync())
            {
                Log.Information("Starting transaction");
                session.StartTransaction();

                try
                {
                    var songPrevUpdate = UpdateSongPosition(documentId, x => x.Id == songToUpdate.Id, x => x.Songs[-1].Previous, newPrevious != null ? newPrevious.Id : null);
                    var songNextUpdate = UpdateSongPosition(documentId, x => x.Id == songToUpdate.Id, x => x.Songs[-1].Next, newNext != null ? newNext.Id : null);                 
                    var oldPrevUpdate = UpdateSongPosition(documentId, x => x.Id == songToUpdate.Previous, x => x.Songs[-1].Next, songToUpdate.Next);
                    var oldNextUpdate = UpdateSongPosition(documentId, x => x.Id == songToUpdate.Next, x => x.Songs[-1].Previous, songToUpdate.Previous);
                    var newPrevUpdate = UpdateSongPosition(documentId, x => x.SongId == playlistSong.Previous, x => x.Songs[-1].Next, songToUpdate.Id);
                    var newNextUpdate = UpdateSongPosition(documentId, x => x.SongId == playlistSong.Next, x => x.Songs[-1].Previous, songToUpdate.Id);

                    await _collection.BulkWriteAsync(new WriteModel<CustomPlaylist>[] 
                    { 
                        songPrevUpdate, songNextUpdate, oldPrevUpdate, oldNextUpdate, newPrevUpdate, newNextUpdate 
                    });

                    await session.CommitTransactionAsync();

                    Log.Information("Transaction is completed");
                    Log.Debug("{method} is finished successfully in {service}", nameof(Update), nameof(PlaylistSongService));
                    return true;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    Log.Error("Can't update song position for song: {songId} in playlist: {id}. Transaction is aborted", playlistSong.Id, documentId);
                    throw new ApplicationException("Can't update this song");
                }
            }
        }

        private UpdateOneModel<CustomPlaylist> UpdateSongPosition(Guid playlistId, Expression<Func<PlaylistSong, bool>> filter,
            Expression<Func<CustomPlaylist, Guid?>> field, Guid? value = null)
        {
            var builder = Builders<CustomPlaylist>.Filter;
            var songFilter = builder.And(builder.Eq(x => x.Id, playlistId), builder.ElemMatch(x => x.Songs, filter));

            var update = Builders<CustomPlaylist>.Update
                .Set(field, value);

            return new UpdateOneModel<CustomPlaylist>(songFilter, update);
        }

        public async Task<bool> Delete(Guid documentId, Guid creatorId, Guid id)
        {
            Log.Debug("{method} is started in {service}", nameof(Delete), nameof(PlaylistSongService));
            var songToDelete = await Get(documentId, id) as PlaylistSong;

            var builder = Builders<CustomPlaylist>.Filter;
            var songFilter = Builders<CustomPlaylist>.Filter
                .And(builder.Eq(x => x.Id, documentId), builder.Eq(x => x.OwnerId, creatorId));

            var update = Builders<CustomPlaylist>.Update
                .PullFilter(x => x.Songs, x => x.Id == songToDelete.Id);

            using(var session = await _dbContext.StartSessionAsync())
            {
                Log.Information("Starting transaction");
                session.StartTransaction();

                try
                {
                    var deleteSong = new UpdateOneModel<CustomPlaylist>(songFilter, update);
                    var updatePrev = UpdateSongPosition(documentId, x => x.Id == songToDelete.Previous,
                        x => x.Songs[-1].Next, songToDelete.Next);
                    var updateNext = UpdateSongPosition(documentId, x => x.Id == songToDelete.Next,
                        x => x.Songs[-1].Previous, songToDelete.Previous);

                    await _collection.BulkWriteAsync(new WriteModel<CustomPlaylist>[]
                    {
                        deleteSong, updatePrev, updateNext
                    });

                    await session.CommitTransactionAsync();

                    Log.Information("Transaction is completed");
                    Log.Debug("{method} is finished successfully in {service}", nameof(Delete), nameof(PlaylistSongService));
                    return true;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    Log.Error("Can't delete song: {songId} in playlist: {id}. Transaction is aborted", songToDelete.Id, documentId);
                    throw new ApplicationException("Can't delete this song");
                }
            }
        }
    }
}
