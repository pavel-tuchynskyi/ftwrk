using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetPlaylists
{
    public class GetPlaylistsQueryHandler : IRequestHandler<GetPlaylistsQuery, PagedList<CustomPlaylistDetailsDto>>
    {
        private readonly IPlaylistService _playlistService;

        public GetPlaylistsQueryHandler(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        public async Task<PagedList<CustomPlaylistDetailsDto>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            var playlists = await _playlistService.GetAll(request);

            return playlists;
        }
    }
}
