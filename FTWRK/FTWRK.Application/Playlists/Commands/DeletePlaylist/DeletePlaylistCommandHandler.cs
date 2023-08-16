using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, Unit>
    {
        private readonly IPlaylistService _playlistService;
        private readonly Guid _userId;

        public DeletePlaylistCommandHandler(IPlaylistService playlistService, IUserContextService userContext)
        {
            _playlistService = playlistService;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _playlistService.GetById(request.Id, PlaylistType.Custom);

            if (_userId != playlist.OwnerId)
            {
                throw new BadRequestException("User isn't creator of this album");
            }

            await _playlistService.Delete(request.Id);

            return Unit.Value;
        }
    }
}
