using AutoMapper;
using FTWRK.Application.Account.Commands.ExternalSignInUser;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class ExternalSignInUserDto : IMap<ExternalSignInUserCommand>
    {
        public string Provider { get; set; }
        public string Token { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExternalSignInUserCommand, ExternalSignInUserDto>();
        }
    }
}
