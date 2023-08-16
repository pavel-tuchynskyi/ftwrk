using AutoMapper;
using FTWRK.Application.Account.Queries.SignInUser;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class UserSignInDto : IMap<SignInUserCommand>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SignInUserCommand, UserSignInDto>();
        }
    }
}
