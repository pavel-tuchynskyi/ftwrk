using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Albums;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IAlbumService
    {
        Task<PagedList<AlbumDto>> GetAll(QueryParameters parameters);
        Task<AlbumDetailsDto> GetById(Guid id);
        Task<bool> Add(Album album);
        Task<bool> Update(Guid id, Album album);
        Task<bool> Delete(Guid id);
    }
}
