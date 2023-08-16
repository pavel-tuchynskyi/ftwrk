namespace FTWRK.Application.Common.DTO.Playlists
{
    public class PlaylistDetailsDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsCustom { get; set; }
    }
}
