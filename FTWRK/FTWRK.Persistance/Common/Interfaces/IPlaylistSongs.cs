using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Persistance.Common.Interfaces
{
    public interface IPlaylistSongs
    {
        Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId);
    }
}
