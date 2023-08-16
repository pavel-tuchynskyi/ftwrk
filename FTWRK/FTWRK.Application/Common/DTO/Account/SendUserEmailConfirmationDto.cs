using AutoMapper;
using FTWRK.Application.Account.Queries.SendUserEmailConfirmation;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class SendUserEmailConfirmationDto : IMap<SendUserEmailConfirmationQuery>
    {
        public string Email { get; set; }
        public string Link { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SendUserEmailConfirmationQuery, SendUserEmailConfirmationDto>();
        }
    }
}
