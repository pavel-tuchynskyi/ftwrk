using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.DeleteSong
{
    public class DeletePlaylistSongCommandHandler : IRequestHandler<DeletePlaylistSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly Guid _userId;

        public DeletePlaylistSongCommandHandler(ISongServiceFactory serviceFactory, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(DeletePlaylistSongCommand request, CancellationToken cancellationToken)
        {
            var songService = _serviceFactory.GetSongService(SongType.Playlist);
            await songService.Delete(request.PlaylistId, _userId, request.SongId);

            return Unit.Value;
        }
    }
}
