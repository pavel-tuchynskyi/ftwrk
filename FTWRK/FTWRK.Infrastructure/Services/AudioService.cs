using FTWRK.Application.Common.Interfaces;
using FTWRK.Infrastructure.Common.Helpers;
using FTWRK.Infrastructure.Common.Models;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Infrastructure.Services
{
    public class AudioService : IAudioService
    {
        public async Task<TimeSpan> GetSongDuration(IFormFile formFile)
        {
            var songBytes = await SongHelper.GetBytes(formFile);
            var fileAbstraction = new FileAbstraction(formFile.FileName, songBytes);

            using (var file = TagLib.File.Create(fileAbstraction))
            {
                var songDuration = file.Properties.Duration;

                return songDuration;
            }
        }
    }
}
