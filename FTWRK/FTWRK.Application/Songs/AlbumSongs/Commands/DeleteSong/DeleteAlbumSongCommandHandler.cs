using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.DeleteSong
{
    public class DeleteAlbumSongCommandHandler : IRequestHandler<DeleteAlbumSongCommand, Unit>
    {
        private readonly ISongServiceFactory _serviceFactory;
        private readonly Guid _userId;

        public DeleteAlbumSongCommandHandler(ISongServiceFactory serviceFactory, IUserContextService userContext)
        {
            _serviceFactory = serviceFactory;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(DeleteAlbumSongCommand request, CancellationToken cancellationToken)
        {
            var songService = _serviceFactory.GetSongService(SongType.Album);
            await songService.Delete(request.AlbumId, _userId, request.SongId);

            return Unit.Value;
        }
    }
}
