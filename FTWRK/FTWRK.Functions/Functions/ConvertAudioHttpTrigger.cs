using FTWRK.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FTWRK.Functions
{
    public class ConvertAudioHttpTrigger
    {
        private readonly IAudioConverter _audioConverter;

        public ConvertAudioHttpTrigger(IAudioConverter audioConverter)
        {
            _audioConverter = audioConverter;
        }

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [FunctionName("ConvertAudioHttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("ConvertAudioHttpTrigger function started to process a request.");

            var songToConvert = req.Form.Files[0];
            var convertedSongBytes = await _audioConverter.ConvertToOgg(songToConvert);

            log.LogInformation("ConvertAudioHttpTrigger function processed a request.");

            return new OkObjectResult(convertedSongBytes);
        }
    }
}
