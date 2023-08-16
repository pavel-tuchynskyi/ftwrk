using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Albums.Commands.DeleteAlbum
{
    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, Unit>
    {
        private readonly IAlbumService _albumService;
        private readonly Guid _userId;

        public DeleteAlbumCommandHandler(IAlbumService albumService, IUserContextService userContext)
        {
            _albumService = albumService;
            _userId = userContext.GetUserId();
        }
        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _albumService.GetById(request.Id);

            if(album.CreatorId != _userId)
            {
                throw new BadRequestException("Can't delete this album");
            }

            var result = await _albumService.Delete(request.Id);

            return Unit.Value;
        }
    }
}
