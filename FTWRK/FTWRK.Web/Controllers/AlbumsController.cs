using FTWRK.Application.Albums.Commands.AddAlbum;
using FTWRK.Application.Albums.Commands.DeleteAlbum;
using FTWRK.Application.Albums.Commands.UpdateAlbum;
using FTWRK.Application.Albums.Queries.GetAlbumById;
using FTWRK.Application.Albums.Queries.GetAlbums;
using FTWRK.Application.Common.DTO.Albums;
using FTWRK.Application.Common.Models;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    public class AlbumsController : BaseController
    {
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseResult<PagedList<AlbumDto>>> GetAlbums(GetAlbumsQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PagedList<AlbumDto>>(200, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<HttpResponseResult<AlbumDetailsDto>> GetAlbumById(Guid id)
        {
            var query = new GetAlbumByIdQuery() { Id = id };
            var result = await Mediator.Send(query);

            return new HttpResponseResult<AlbumDetailsDto>(200, result);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Guid>> AddAlbum([FromForm]AddAlbumCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Guid>(201, result);
        }

        [HttpPut]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Unit>> UpdateAlbum([FromForm]UpdateAlbumCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Unit>> DeleteAlbum(Guid id)
        {
            var command = new DeleteAlbumCommand() { Id = id };
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }
    }
}
