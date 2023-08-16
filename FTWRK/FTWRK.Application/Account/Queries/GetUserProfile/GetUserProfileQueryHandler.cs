using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Account.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDetailsDto>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public GetUserProfileQueryHandler(IUserManager userManager, IMapper mapper, IUserContextService userContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userId = userContext.GetUserId();
        }
        public async Task<UserDetailsDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUser(_userId);

            var userDto = _mapper.Map<UserDetailsDto>(user);

            return userDto;
        }
    }
}
