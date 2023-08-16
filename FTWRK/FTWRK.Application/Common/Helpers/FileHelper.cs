using Microsoft.AspNetCore.Http;

namespace FTWRK.Application.Common.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> SerializeImageAsync(IFormFile image)
        {
            var imageBytes = new byte[image.Length];

            using (var fs = image.OpenReadStream())
            {
                await fs.ReadAsync(imageBytes, 0, (int)image.Length);
                await fs.FlushAsync();
            }

            return imageBytes;
        }
    }
}
