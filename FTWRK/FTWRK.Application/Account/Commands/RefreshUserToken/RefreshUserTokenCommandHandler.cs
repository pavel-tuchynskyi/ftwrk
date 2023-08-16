using AutoMapper;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.RefreshUserToken
{
    public class RefreshUserTokenCommandHandler : IRequestHandler<RefreshUserTokenCommand, Token>
    {
        private readonly ISignInManager _signInManager;
        private readonly IMapper _mapper;

        public RefreshUserTokenCommandHandler(ISignInManager signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<Token> Handle(RefreshUserTokenCommand request, CancellationToken cancellationToken)
        {
            var token = _mapper.Map<Token>(request);
            var result = await _signInManager.RefreshUserToken(token);

            return result;
        }
    }
}
