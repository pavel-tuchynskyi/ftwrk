using FTWRK.Domain.Attributes;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Domain.Entities.Playlists
{
    [BsonCollection("Playlists")]
    public class CustomPlaylist: Playlist
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ImageBlob Poster { get; set; }
        public override bool IsCustom => true;

        public CustomPlaylist(Guid id, Guid ownerId, List<PlaylistSong> songs, string title, string description, ImageBlob poster)
            : base(id, ownerId, songs)
        {
            Title = title;
            Description = description;
            Poster = poster;
        }

        public CustomPlaylist()
        {

        }
    }
}
