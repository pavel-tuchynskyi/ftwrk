using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Models;
using FTWRK.Application.Playlists.Commands.CreatePlaylist;
using FTWRK.Application.Playlists.Commands.DeletePlaylist;
using FTWRK.Application.Playlists.Commands.UpdatePlaylist;
using FTWRK.Application.Playlists.Queries.GetCustomPlaylist;
using FTWRK.Application.Playlists.Queries.GetFavoritePlaylist;
using FTWRK.Application.Playlists.Queries.GetPlaylists;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    [Authorize]
    public class PlaylistsController : BaseController
    {
        [HttpPost]
        public async Task<HttpResponseResult<PagedList<CustomPlaylistDetailsDto>>> GetPlaylists(GetPlaylistsQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PagedList<CustomPlaylistDetailsDto>>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Guid>> AddPlaylist([FromForm] CreatePlaylistCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Guid>(200, result);
        }

        [HttpGet("{id}")]
        public async Task<HttpResponseResult<CustomPlaylistDetailsDto>> GetCustomPlaylist(Guid id)
        {
            var query = new GetCustomPlaylistQuery() { Id = id };
            var result = await Mediator.Send(query);

            return new HttpResponseResult<CustomPlaylistDetailsDto>(200, result);
        }

        [HttpGet]
        public async Task<HttpResponseResult<PlaylistDetailsDto>> GetFavoritePlaylist()
        {
            var query = new GetFavoritePlaylistQuery();
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PlaylistDetailsDto>(200, result);
        }

        [HttpPut]
        public async Task<HttpResponseResult<Unit>> UpdatePlaylist([FromForm] UpdatePlaylistCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpDelete("{id}")]
        public async Task<HttpResponseResult<Unit>> DeletePlaylist(Guid id)
        {
            var command = new DeletePlaylistCommand() { Id = id };
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }
    }
}
