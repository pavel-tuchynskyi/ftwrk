using AspNetCore.CacheOutput;
using FTWRK.Application.Common.DTO.Images;
using FTWRK.Application.Images.Commands.UploadPicture;
using FTWRK.Application.Images.Queries.GetPictureByName;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    public class ImageController : BaseController
    {
        private const int CacheDurationInSec = 2592000;

        [HttpGet]
        [CacheOutput(ClientTimeSpan = CacheDurationInSec, MustRevalidate = true)]
        public async Task<IActionResult> GetPictureByName([FromQuery]GetPictureByNameQuery query)
        {
            var result = await Mediator.Send(query);

            return File(result.ImageBytes, result.ImageType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [InvalidateCacheOutput(nameof(GetPictureByName))]
        public async Task<HttpResponseResult<Unit>> UploadPicture([FromForm]UploadPictureCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }
    }
}
