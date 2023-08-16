using FTWRK.Application.Common.Interfaces;
using FTWRK.Infrastructure.Common.Helpers;
using FTWRK.Infrastructure.Common.Models;
using FTWRK.Infrastructure.Services.AudiConverterChain;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace FTWRK.Infrastructure.Services
{
    public class AudioConverter : IAudioConverter
    {
        public async Task<byte[]> ConvertToOgg(IFormFile formFile)
        {
            Log.Debug("{method} started in service {service}", nameof(ConvertToOgg), nameof(AudioConverter));
            var songBytes = await SongHelper.GetBytes(formFile);
            var songInfo = SongHelper.GetSongProperties(formFile.FileName, songBytes);

            var audioConverter = new AudioEncoder(songBytes, songInfo);
            var mp3Converter = new Mp3Converter();
            var wavConverter = new WavConverter();
            mp3Converter.Successor = wavConverter;

            var oggBytes = await mp3Converter.Handle(audioConverter);

            Log.Debug("{method} is finished successfully in {service}", nameof(ConvertToOgg), nameof(AudioConverter));
            return oggBytes;
        }
    }
}
