using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Images;
using FTWRK.Domain.Entities.Songs;

namespace FTWRK.Application.Common.DTO.Account
{
    public class UserDetailsDto : UserDto, IMap<SongArtist>
    {
        public string UserName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDetailsDto, SongArtist>()
                .ForMember(x => x.ArtistName, x => x.MapFrom(y => y.FullName));

            profile.CreateMap<IUser, UserDetailsDto>();
        }
    }
}
