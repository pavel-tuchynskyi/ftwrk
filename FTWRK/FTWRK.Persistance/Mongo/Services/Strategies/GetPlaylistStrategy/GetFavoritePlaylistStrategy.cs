using AutoMapper;
using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistStrategy
{
    public class GetFavoritePlaylistStrategy : IPlaylist
    {
        public readonly PlaylistType playlistType = PlaylistType.Favorite;
        private readonly IMongoCollection<CustomPlaylist> _collection;

        public GetFavoritePlaylistStrategy(IMongoCollection<CustomPlaylist> collection)
        {
            _collection = collection;
        }
        public async Task<PlaylistDetailsDto> GetPlaylist(Guid id)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateProjection<CustomPlaylist, PlaylistDetailsDto>();
            });

            var playlist = await _collection
                .AsQueryable()
                .Where(x => x.OwnerId == id && x.IsCustom == false)
                .ProjectTo<CustomPlaylist, PlaylistDetailsDto>(configuration)
                .FirstOrDefaultAsync();

            return playlist;
        }
    }
}
