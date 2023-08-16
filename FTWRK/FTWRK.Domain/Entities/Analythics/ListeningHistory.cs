using FTWRK.Domain.Attributes;

namespace FTWRK.Domain.Entities.Analythics
{
    [BsonCollection("ListeningHistory")]
    public class ListeningHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid AlbumId { get; set; }
        public DateTime ListeningDate { get; set; } = DateTime.Now;
    }
}
