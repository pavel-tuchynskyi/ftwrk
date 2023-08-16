using AutoMapper;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Albums;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Albums
{
    public class AlbumDto: IMap<Album>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public ImageBlob Poster { get; set; }
        public AlbumType AlbumType { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Album, AlbumDto>();
        }
    }
}
