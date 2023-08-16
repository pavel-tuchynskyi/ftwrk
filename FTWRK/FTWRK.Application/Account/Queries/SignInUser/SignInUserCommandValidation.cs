using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Queries.SignInUser
{
    public class SignInUserCommandValidation : CustomAbstractValidator<SignInUserCommand>
    {
        public SignInUserCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Invalid password");
        }
    }
}
