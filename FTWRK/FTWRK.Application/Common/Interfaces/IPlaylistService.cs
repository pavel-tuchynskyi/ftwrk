using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IPlaylistService
    {
        Task<PagedList<CustomPlaylistDetailsDto>> GetAll(QueryParameters parameters);
        Task<bool> Add(Playlist playlist);
        Task<PlaylistDetailsDto> GetById(Guid id, PlaylistType playlistType);
        Task<bool> Update(Guid id, CustomPlaylist playlist);
        Task<bool> Delete(Guid id);
    }
}
