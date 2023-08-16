using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.RefreshUserToken
{
    public class RefreshUserTokenCommandValidation : CustomAbstractValidator<RefreshUserTokenCommand>
    {
        public RefreshUserTokenCommandValidation()
        {
            RuleFor(x => x.AccessToken).NotEmpty().WithMessage("Access token is required");
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
        }
    }
}
