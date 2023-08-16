using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IAudioConverter
    {
        Task<byte[]> ConvertToOgg(IFormFile formFile);
    }
}
