using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Mappings;
using FTWRK.Application.Common.Models;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Songs
{
    public class SongArtistDto : IMap<IUser>
    {
        public Guid Id { get; set; }
        public string ArtistName { get; set; }
        public ImageBlob ProfilePicture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IUser, SongArtistDto>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.ArtistName, x => x.MapFrom(y => y.FullName));

            profile.CreateMap<PagedList<IUser>, PagedList<SongArtistDto>>();
        }
    }
}
