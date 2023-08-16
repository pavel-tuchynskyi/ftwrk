using Azure.Storage.Blobs;

namespace FTWRK.Persistance.Common.Interfaces
{
    public interface IBlobStorageContext
    {
        Task<BlobContainerClient> GetContainerClient<T>();
    }
}
