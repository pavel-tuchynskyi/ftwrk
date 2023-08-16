using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Account.Queries.GetPublicProfile
{
    public class GetPublicProfileQueryHandler : IRequestHandler<GetPublicProfileQuery, UserDto>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GetPublicProfileQueryHandler(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserDto> Handle(GetPublicProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUser(request.Id);

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
