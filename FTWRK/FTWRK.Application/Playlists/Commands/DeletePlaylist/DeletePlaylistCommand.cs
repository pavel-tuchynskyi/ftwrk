using MediatR;

namespace FTWRK.Application.Playlists.Commands.DeletePlaylist
{
    public class DeletePlaylistCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
