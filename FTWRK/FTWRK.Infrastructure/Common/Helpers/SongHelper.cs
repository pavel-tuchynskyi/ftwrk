using FTWRK.Infrastructure.Common.Models;
using Microsoft.AspNetCore.Http;

namespace FTWRK.Infrastructure.Common.Helpers
{
    public static class SongHelper
    {
        public static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            var imageBytes = new byte[formFile.Length];

            using (var fs = formFile.OpenReadStream())
            {
                await fs.ReadAsync(imageBytes, 0, imageBytes.Length);
                await fs.FlushAsync();
            }

            return imageBytes;
        }

        public static SongProperties GetSongProperties(string fileName, byte[] songBytes)
        {
            var fileAbstraction = new FileAbstraction(fileName, songBytes);
            using (var file = TagLib.File.Create(fileAbstraction))
            {
                var fileFormat = GetFormatFromMimeType(file.MimeType);

                var songProps = new SongProperties()
                {
                    Format = fileFormat,
                    Bitrate = file.Properties.AudioBitrate,
                    SampleRate = file.Properties.AudioSampleRate,
                    Channels = file.Properties.AudioChannels,
                };

                return songProps;
            }
        }

        private static string GetFormatFromMimeType(string mimeType)
        {
            var fileSpan = mimeType.AsSpan();
            var index = fileSpan.LastIndexOf('/');
            var format = fileSpan.Slice(index + 1);
            return format.ToString();
        }
    }
}
