using FTWRK.Application.Common.DTO.Playlists;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Playlists;
using MediatR;

namespace FTWRK.Application.Playlists.Queries.GetFavoritePlaylist
{
    public class GetFavoritePlaylistQueryHandler : IRequestHandler<GetFavoritePlaylistQuery, PlaylistDetailsDto>
    {
        private readonly IPlaylistService _playlistService;
        private readonly Guid _userId;

        public GetFavoritePlaylistQueryHandler(IPlaylistService playlistService, IUserContextService userContext)
        {
            _playlistService = playlistService;
            _userId = userContext.GetUserId();
        }
        public async Task<PlaylistDetailsDto> Handle(GetFavoritePlaylistQuery request, CancellationToken cancellationToken)
        {
            var playlist = await _playlistService.GetById(_userId, PlaylistType.Favorite);

            return playlist;
        }
    }
}
