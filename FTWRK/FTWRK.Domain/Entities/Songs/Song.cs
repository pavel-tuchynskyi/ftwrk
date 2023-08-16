using FTWRK.Domain.Common;

namespace FTWRK.Domain.Entities.Songs
{
    public class Song : SongBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<SongArtist> Artists { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
