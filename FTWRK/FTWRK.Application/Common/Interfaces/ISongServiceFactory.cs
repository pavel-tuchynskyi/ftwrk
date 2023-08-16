using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Common.Interfaces
{
    public interface ISongServiceFactory
    {
        ISongService GetSongService(SongType songType);
    }
}
