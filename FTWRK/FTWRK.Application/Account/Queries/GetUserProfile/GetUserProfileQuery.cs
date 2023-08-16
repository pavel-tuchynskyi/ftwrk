using FTWRK.Application.Common.DTO.Account;
using MediatR;

namespace FTWRK.Application.Account.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserDetailsDto>
    {
    }
}
