using FTWRK.Application.Common.Helpers;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Driver;

namespace FTWRK.Persistance.Mongo.Services.Strategies.GetPlaylistStrategy
{
    public class GetPlaylistStrategyResolver
    {
        private readonly IMongoCollection<CustomPlaylist> _collection;

        public GetPlaylistStrategyResolver(IMongoCollection<CustomPlaylist> collection)
        {
            _collection = collection;
        }
        public IPlaylist GetPlaylistStrategy(PlaylistType playlistType)
        {
            var playlistStrategy = StrategyHelper.GetStrategies<PlaylistType, IPlaylist>(typeof(GetPlaylistStrategyResolver), _collection);

            return playlistStrategy[playlistType];
        }
    }
}
