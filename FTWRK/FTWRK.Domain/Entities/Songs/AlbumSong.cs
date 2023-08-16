using FTWRK.Domain.Common;

namespace FTWRK.Domain.Entities.Songs
{
    public class AlbumSong: SongBase
    {
        public SongBlob SongBlob { get; set; } = new SongBlob();
        public Song Song { get; set; } = new Song();
    }
}
