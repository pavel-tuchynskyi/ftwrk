using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.DTO.Songs
{
    public class SongDto : IMap<Song>
    {
        public Guid Id { get; set; }
        public List<SongArtist> Artists { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsArchived { get; set; }
        public Guid AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public bool IsFavorite { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Song, SongDto>();
        }
    }
}
