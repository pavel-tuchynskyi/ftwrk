using AutoMapper;
using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistStrategy;
using MongoDB.Driver;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IMongoCollection<CustomPlaylist> _collection;
        public PlaylistService(IMongoContext dbContext)
        {
            _collection = dbContext.GetCollection<CustomPlaylist>();
        }

        public async Task<PagedList<CustomPlaylistDetailsDto>> GetAll(QueryParameters parameters)
        {
            Log.Debug("{method} is started in {service}", nameof(GetAll), nameof(PlaylistService));

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateProjection<CustomPlaylist, CustomPlaylistDetailsDto>();
            });

            var result = await _collection
                .AsQueryable()
                .Filter(parameters.Filter)
                .Sort(parameters.OrderBy)
                .ProjectTo<CustomPlaylist, CustomPlaylistDetailsDto>(configuration)
                .ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            Log.Debug("{method} is finished successfully in {service}", nameof(GetAll), nameof(PlaylistService));

            return result;
        }

        public async Task<bool> Add(Playlist playlist)
        {
            Log.Debug("{method} is started in {service}", nameof(Add), nameof(PlaylistService));

            await _collection.InsertOneAsync((CustomPlaylist)playlist);

            Log.Debug("{method} is finished successfully in {service}", nameof(Add), nameof(PlaylistService));

            return true;
        }

        public async Task<PlaylistDetailsDto> GetById(Guid id, PlaylistType playlistType)
        {
            Log.Debug("{method} is started in {service}", nameof(GetById), nameof(PlaylistService));

            var resolver = new GetPlaylistStrategyResolver(_collection);
            var strategy = resolver.GetPlaylistStrategy(playlistType);
            var result = await strategy.GetPlaylist(id);

            if (result == null)
            {
                Log.Error("Can't find playlist with id: {id}", id);
                throw new NotFoundException("Can't find thid playlist");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(GetById), nameof(PlaylistService));
            return result;
        }

        public async Task<bool> Update(Guid id, CustomPlaylist playlist)
        {
            Log.Debug("{method} is started in {service}", nameof(Update), nameof(PlaylistService));

            var updateDef = Builders<CustomPlaylist>.Update.CreateUpdateDefinition(playlist);

            var result = await _collection.UpdateOneAsync(x => x.Id == id && x.OwnerId == playlist.OwnerId, updateDef);

            if (result.ModifiedCount == 0)
            {
                Log.Error("Can't update playlist with id: {id}", id);
                throw new ApplicationException("Can't update this playlist");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Update), nameof(PlaylistService));
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            Log.Debug("{method} is started in {service}", nameof(Delete), nameof(PlaylistService));

            var result = await _collection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount == 0)
            {
                Log.Error("Can't delete playlist with id: {id}", id);
                throw new ApplicationException("Can't delete this playlist");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(Delete), nameof(PlaylistService));
            return true;
        }
    }
}
