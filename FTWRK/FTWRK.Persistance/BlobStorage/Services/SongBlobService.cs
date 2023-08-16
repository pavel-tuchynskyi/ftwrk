using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Songs;
using FTWRK.Persistance.Common.Interfaces;

namespace FTWRK.Persistance.BlobStorage.Services
{
    public class SongBlobService : ISongBlobService
    {
        private readonly IBlobStorageContext _storageContext;

        public SongBlobService(IBlobStorageContext storageContext)
        {
            _storageContext = storageContext;
        }

        public async Task<SongBlob> Get(Guid id)
        {
            var client = await _storageContext.GetContainerClient<SongBlob>();
            var blobClient = client.GetBlobClient(id.ToString() + ".ogg");

            var songBlob = new SongBlob();
            if(await blobClient.ExistsAsync())
            {
                using (var ms = new MemoryStream())
                {
                    blobClient.DownloadTo(ms);
                    songBlob.SongBytes = ms.ToArray();
                    songBlob.Id = id;
                }
            }

            return songBlob;
        }

        public async Task<bool> IsExist(Guid id)
        {
            var client = await _storageContext.GetContainerClient<SongBlob>();
            var blobClient = client.GetBlobClient(id.ToString() + ".ogg");

            return await blobClient.ExistsAsync();
        }

        public async Task<bool> Upload(SongBlob songBlob)
        {
            var client = await _storageContext.GetContainerClient<SongBlob>();

            using(var stream = new MemoryStream(songBlob.SongBytes))
            {
                await client.UploadBlobAsync(songBlob.Id.ToString() + ".ogg", stream);

                return true;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            var client = await _storageContext.GetContainerClient<SongBlob>();
            
            var result = await client.DeleteBlobIfExistsAsync(id.ToString() + ".ogg");

            return result;
        }
    }
}
