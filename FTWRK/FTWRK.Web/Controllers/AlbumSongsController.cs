using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Models;
using FTWRK.Application.Songs.AlbumSongs.Commands.AddSong;
using FTWRK.Application.Songs.AlbumSongs.Commands.DeleteSong;
using FTWRK.Application.Songs.AlbumSongs.Commands.UpdateSong;
using FTWRK.Application.Songs.AlbumSongs.Queries.GetSong;
using FTWRK.Application.Songs.AlbumSongs.Queries.GetSongArtists;
using FTWRK.Application.Songs.AlbumSongs.Queries.GetSongs;
using FTWRK.Web.Configuration;
using FTWRK.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FTWRK.Web.Controllers
{
    public class AlbumSongsController : BaseController
    {
        [HttpPost]
        [Authorize]
        public async Task<HttpResponseResult<PagedList<SongDto>>> GetSongs(GetAlbumSongsQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PagedList<SongDto>>(200, result);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Unit>> AddSong([FromForm]AddAlbumSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(201, result);
        }

        [HttpPost]
        [Authorize]
        public async Task<HttpResponseResult<PagedList<SongArtistDto>>> GetArtists(GetArtistsQuery query)
        {
            var result = await Mediator.Send(query);

            return new HttpResponseResult<PagedList<SongArtistDto>>(200, result);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Unit>> UpdateSong(UpdateAlbumSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<HttpResponseResult<Unit>> DeleteSong(DeleteAlbumSongCommand command)
        {
            var result = await Mediator.Send(command);

            return new HttpResponseResult<Unit>(200, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSong([FromQuery]Guid albumId, [FromQuery]Guid songId)
        {
            var query = new GetAlbumSongQuery()
            {
                AlbumId = albumId,
                SongId = songId
            };
            var result = await Mediator.Send(query);

            return File(result.SongBytes, AudioResponseConstants.CONTENT_TYPE, $"{result.Id}.{AudioResponseConstants.AUDIO_FORMAT}", enableRangeProcessing: true);
        }
    }
}
