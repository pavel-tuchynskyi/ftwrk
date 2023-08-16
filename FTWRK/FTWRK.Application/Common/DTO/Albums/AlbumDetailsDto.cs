using AutoMapper;
using FTWRK.Application.Common.DTO.Songs;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Images;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.DTO.Albums
{
    public class AlbumDetailsDto : IMap<Album>
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public List<string> Genres { get; set; }
        public AlbumType AlbumType { get; set; }
        public ImageBlob Poster { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Album, AlbumDetailsDto>();
        }
    }
}
