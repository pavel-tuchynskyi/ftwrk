using FTWRK.Application.Images.Commands.UploadPicture;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<Image> GetImageByName(string name);
        Task<bool> UploadPicture(Image image);
    }
}
