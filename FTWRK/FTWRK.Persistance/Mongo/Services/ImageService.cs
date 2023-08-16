using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Domain.Entities.Images;
using FTWRK.Persistance.Common.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace FTWRK.Persistance.Mongo.Services
{
    public class ImageService : IImageService
    {
        private readonly IMongoCollection<Image> _imageCollection;

        public ImageService(IMongoContext dbContext)
        {
            _imageCollection = dbContext.GetCollection<Image>();
        }
        public async Task<Image> GetImageByName(string name)
        {
            Log.Debug("{method} is started in {service}", nameof(GetImageByName), nameof(ImageService));

            var image = await _imageCollection.Find(x => x.Name == name).SingleOrDefaultAsync();

            if(image == null)
            {
                Log.Error("Can't find image with name: {name}", name);
                throw new NotFoundException("Can't find this image");
            }

            Log.Debug("{method} is finished successfully in {service}", nameof(GetImageByName), nameof(ImageService));

            return image;
        }

        public async Task<bool> UploadPicture(Image image)
        {
            Log.Debug("{method} is started in {service}", nameof(UploadPicture), nameof(ImageService));

            await _imageCollection.InsertOneAsync(image);

            Log.Debug("{method} is finished successfully in {service}", nameof(UploadPicture), nameof(ImageService));

            return true;
        }
    }
}
