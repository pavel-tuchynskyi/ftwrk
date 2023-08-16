using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;

namespace FTWRK.Persistance.Mongo.Services.Factory
{
    public class SongServiceFactory : ISongServiceFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SongServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ISongService GetSongService(SongType songType)
        {
            switch (songType)
            {
                case SongType.Playlist:
                    return (ISongService)serviceProvider.GetService(typeof(PlaylistSongService));
                case SongType.Album:
                    return (ISongService)serviceProvider.GetService(typeof(AlbumSongService));
                default:
                    return null;
            }
        }
    }
}
