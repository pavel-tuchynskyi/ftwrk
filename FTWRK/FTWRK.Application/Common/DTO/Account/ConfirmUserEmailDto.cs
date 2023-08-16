using AutoMapper;
using FTWRK.Application.Account.Commands.ConfirmUserEmail;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class ConfirmUserEmailDto : IMap<ConfirmUserEmailCommand>
    {
        public string Email { get; set; }
        public string Code { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ConfirmUserEmailCommand, ConfirmUserEmailDto>();
        }
    }
}
