using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Commands.DeleteSong
{
    public class DeleteAlbumSongCommand : IRequest<Unit>
    {
        public Guid AlbumId { get; set; }
        public Guid SongId { get; set; }
    }
}
