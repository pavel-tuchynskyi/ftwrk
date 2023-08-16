namespace FTWRK.Domain.Entities.Playlists
{
    public class Playlist
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OwnerId { get; set; }
        public List<PlaylistSong> Songs { get; set; }
        public virtual bool IsCustom { get; set; } = false;

        public Playlist(Guid id, Guid ownerId, List<PlaylistSong> songs)
        {
            Id = id;
            OwnerId = ownerId;
            Songs = songs;
        }

        public Playlist()
        {
        }
    }
}
