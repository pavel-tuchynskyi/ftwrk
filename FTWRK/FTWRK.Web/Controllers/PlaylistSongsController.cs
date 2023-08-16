using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Application.Songs.PlaylistSongs.Commands.AddSong;
using FTWRK.Application.Songs.PlaylistSongs.Commands.DeleteSong;
using FTWRK.Application.Songs.PlaylistSongs.Commands.UpdateSong;
using FTWRK.Application.Songs.PlaylistSongs.Queries.GetPlaylistSongs;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    [Authorize]
    public class PlaylistSongsController : BaseController
    {
        [HttpPost]
        public async Task<HttpResponseResult<PagedList<SongDto>>> GetSongs(GetPlaylistSongsQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PagedList<SongDto>>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Unit>> AddSong(AddPlaylistSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpPut]
        public async Task<HttpResponseResult<Unit>> UpdateSong(UpdatePlaylistSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpPost]
        public async Task<HttpResponseResult<Unit>> DeleteSong(DeletePlaylistSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }
    }
}
