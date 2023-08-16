using AutoMapper;
using FTWRK.Application.Account.Commands.ResetPassword;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class ResetPasswordDto : IMap<ResetPasswordCommand>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ResetPasswordCommand, ResetPasswordDto>();
        }
    }
}
