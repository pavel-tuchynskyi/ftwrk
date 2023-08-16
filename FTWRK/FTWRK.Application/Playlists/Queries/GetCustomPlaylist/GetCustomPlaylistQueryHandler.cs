using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetCustomPlaylist
{
    public class GetCustomPlaylistQueryHandler : IRequestHandler<GetCustomPlaylistQuery, CustomPlaylistDetailsDto>
    {
        private readonly IPlaylistService _playlistService;

        public GetCustomPlaylistQueryHandler(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task<CustomPlaylistDetailsDto> Handle(GetCustomPlaylistQuery request, CancellationToken cancellationToken)
        {
            var result = await _playlistService.GetById(request.Id, PlaylistType.Custom) as CustomPlaylistDetailsDto;

            return result;
        }
    }
}
