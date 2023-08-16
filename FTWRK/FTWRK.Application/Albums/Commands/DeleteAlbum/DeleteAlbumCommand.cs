using MediatR;

namespace FTWRK.Application.Albums.Commands.DeleteAlbum
{
    public class DeleteAlbumCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
