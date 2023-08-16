using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;

namespace FTWRK.Persistance.Common.Interfaces
{
    public interface IPlaylist
    {
        Task<PlaylistDetailsDto> GetPlaylist(Guid id);
    }
}
