using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Persistance.Common.Models
{
    public class PlaylistAlbumSongs
    {
        public Guid Id { get; set; }
        public Guid SongId { get; set; }
        public Guid? Previous { get; set; }
        public Guid? Next { get; set; }
        public AlbumSongs AlbumSongs { get; set; }
    }

    public class AlbumSongs
    {
        public List<Song> Songs { get; set; }
    }
}
