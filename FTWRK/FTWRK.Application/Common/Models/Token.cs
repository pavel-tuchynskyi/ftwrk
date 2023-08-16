using AutoMapper;
using FTWRK.Application.Account.Commands.RefreshUserToken;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.Models
{
    public class Token : IMap<RefreshUserTokenCommand>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RefreshUserTokenCommand, Token>();
        }
    }
}
