namespace FTWRK.Domain.Entities.Songs
{
    public class SongBlob
    {
        public Guid Id { get; set; }
        public byte[] SongBytes { get; set; }
    }
}
