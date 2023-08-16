using FTWRK.Domain.Attributes;
using FTWRK.Domain.Entities.Images;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Domain.Entities.Albums
{
    [BsonCollection("Albums")]
    public class Album
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<string> Genres { get; set; }
        public AlbumType AlbumType { get; set; }
        public ImageBlob Poster { get; set; }
        public List<Song> Songs { get; set; }
        public bool IsArchived { get; set; } = false;

        public Album()
        {
        }
    }

    public enum AlbumType
    {
        Album,
        EP_Single,
        Compilation
    }
}
