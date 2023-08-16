using FTWRK.Application.Common.DTO.Songs;

namespace FTWRK.Persistance.Common.Models
{
    public class PlaylistSongResult
    {
        public Guid Id { get; set; }
        public Guid? Previous { get; set; }
        public Guid? Next { get; set; }
        public SongDto Song { get; set; }
    }
}
