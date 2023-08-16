using FTWRK.Domain.Entities.Songs;
using MediatR;

namespace FTWRK.Application.Songs.AlbumSongs.Queries.GetSong
{
    public class GetAlbumSongQuery : IRequest<SongBlob>
    {
        public Guid AlbumId { get; set; }
        public Guid SongId { get; set; }
    }
}
