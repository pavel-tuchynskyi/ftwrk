using AutoMapper;
using FTWRK.Application.Account.Queries.ForgetPassword;
using FTWRK.Application.Common.Mappings;

namespace FTWRK.Application.Common.DTO.Account
{
    public class ForgetPasswordDto : IMap<ForgetPasswordQuery>
    {
        public string Email { get; set; }
        public string Link { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ForgetPasswordQuery, ForgetPasswordDto>();
        }
    }
}
