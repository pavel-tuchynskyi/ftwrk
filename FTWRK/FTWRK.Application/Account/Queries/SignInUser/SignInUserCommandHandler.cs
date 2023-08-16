using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Queries.SignInUser
{
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, Token>
    {
        private readonly ISignInManager _signInManager;

        public SignInUserCommandHandler(ISignInManager signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<Token> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.SignInUserAsync(request.Email, request.Password);

            return result;
        }
    }
}
