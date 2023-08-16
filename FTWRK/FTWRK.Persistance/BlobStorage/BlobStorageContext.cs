using Azure.Storage.Blobs;
using FTWRK.Persistance.Common.Interfaces;
using FTWRK.Persistance.Configuration;
using Microsoft.Extensions.Options;

namespace FTWRK.Persistance.BlobStorage
{
    public class BlobStorageContext : IBlobStorageContext
    {
        private readonly BlobStorageOptions _blobStorageOptions;

        public BlobStorageContext(IOptions<BlobStorageOptions> blobStorageOptions)
        {
            _blobStorageOptions = blobStorageOptions.Value;
        }

        public async Task<BlobContainerClient> GetContainerClient<T>()
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageOptions.ConnectionString, new BlobClientOptions(BlobClientOptions.ServiceVersion.V2020_10_02));
            var containerClient = blobServiceClient.GetBlobContainerClient(typeof(T).Name.ToLower());

            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }
    }
}
