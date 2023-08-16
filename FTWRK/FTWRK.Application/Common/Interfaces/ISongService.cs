using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Common;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.Interfaces
{
    public interface ISongService
    {
        Task<PagedList<SongDto>> GetAll(QueryParameters parameters, Guid userId, Guid? documentId);
        Task<SongBase> Get(Guid documentId, Guid? id);
        Task<bool> Add(Guid documentId, Guid creatorId, SongBase song);
        Task<bool> Update(Guid documentId, Guid creatorId, SongBase song);
        Task<bool> Delete(Guid documentId, Guid creatorId, Guid id);
    }
}
