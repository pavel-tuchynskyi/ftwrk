using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Mappings;
using FTWRK.Domain.Entities.Images;

namespace FTWRK.Application.Common.DTO.Account
{
    public class UserDto : IMap<IUser>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public ImageBlob ProfilePicture { get; set; }
        public ImageBlob BackgroundPicture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IUser, UserDto>();
        }
    }
}
