using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Mappings;
using MediatR;

namespace FTWRK.Application.Account.Queries.GetPublicProfile
{
    public class GetPublicProfileQuery : IRequest<UserDto>, IMap<UserDto>
    {
        public Guid Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GetPublicProfileQuery, UserDto>();
        }
    }
}
