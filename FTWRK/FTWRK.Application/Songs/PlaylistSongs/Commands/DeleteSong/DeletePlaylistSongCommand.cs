using MediatR;

namespace FTWRK.Application.Songs.PlaylistSongs.Commands.DeleteSong
{
    public class DeletePlaylistSongCommand : IRequest<Unit>
    {
        public Guid PlaylistId { get; set; }
        public Guid SongId { get; set; }
    }
}
