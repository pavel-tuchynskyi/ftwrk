using FTWRK.Domain.Attributes;
using FTWRK.Domain.Common;

namespace FTWRK.Domain.Entities.Playlists
{
    [BsonCollection("Songs")]
    public class PlaylistSong : SongBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AlbumId { get; set; }
        public Guid SongId { get; set; }
        public Guid? Previous { get; set; }
        public Guid? Next { get; set; }
    }
}
