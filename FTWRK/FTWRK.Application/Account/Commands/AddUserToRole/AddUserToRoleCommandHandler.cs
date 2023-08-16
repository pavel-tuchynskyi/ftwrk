using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.AddUserToRole
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, Token>
    {
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;
        private readonly Guid _userId;

        public AddUserToRoleCommandHandler(IUserManager userManager, IUserContextService userContext, ISignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userId = userContext.GetUserId();
        }
        public async Task<Token> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUser(_userId);

            await _userManager.AddUserToRole(user.Email, request.RoleName);

            var result = await _signInManager.RefreshUserToken(request.Token);

            return result;
        }
    }
}
