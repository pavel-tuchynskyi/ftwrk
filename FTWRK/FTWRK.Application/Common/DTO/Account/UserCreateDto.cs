using AutoMapper;
using FTWRK.Application.Account.Commands.RegisterUser;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class UserCreateDto : IMap<CreateUserCommand>
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserCommand, UserCreateDto>();
        }
    }
}
