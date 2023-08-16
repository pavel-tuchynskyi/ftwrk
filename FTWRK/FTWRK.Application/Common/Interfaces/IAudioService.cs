using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Common.Interfaces
{
    public interface IAudioService
    {
        Task<TimeSpan> GetSongDuration(IFormFile file);
    }
}
