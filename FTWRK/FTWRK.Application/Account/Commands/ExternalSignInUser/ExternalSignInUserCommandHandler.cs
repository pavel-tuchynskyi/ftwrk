using AutoMapper;
using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.ExternalSignInUser
{
    public class ExternalSignInUserCommandHandler : IRequestHandler<ExternalSignInUserCommand, Token>
    {
        private readonly ISignInManager _signInManager;
        private readonly IMapper _mapper;

        public ExternalSignInUserCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<Token> Handle(ExternalSignInUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = _mapper.Map<ExternalSignInUserDto>(request);

            var result = await _signInManager.ExternalUserSignInAsync(userDto);

            return result;
        }
    }
}
