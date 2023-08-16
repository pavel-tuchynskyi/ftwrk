using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.ExternalSignInUser
{
    public class ExternalSignInUserCommandValidation : CustomAbstractValidator<ExternalSignInUserCommand>
    {
        public ExternalSignInUserCommandValidation()
        {
            RuleFor(x => x.Provider).NotEmpty().WithMessage("Privider is required");
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
        }
    }
}
