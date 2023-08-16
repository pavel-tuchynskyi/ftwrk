using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.Interfaces
{
    public interface ISongBlobService
    {
        Task<bool> Upload(SongBlob songBlob);
        Task<bool> Delete(Guid id);
        Task<SongBlob> Get(Guid id);
        Task<bool> IsExist(Guid id);
    }
}
