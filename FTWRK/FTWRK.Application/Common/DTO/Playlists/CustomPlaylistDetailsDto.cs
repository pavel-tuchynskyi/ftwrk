using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Playlists
{
    public class CustomPlaylistDetailsDto : PlaylistDetailsDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public ImageBlob? Poster { get; set; }
        public string OwnerName { get; set; }
    }
}
