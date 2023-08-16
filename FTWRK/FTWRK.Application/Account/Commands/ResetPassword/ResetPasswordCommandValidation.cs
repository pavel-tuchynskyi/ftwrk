using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.ResetPassword
{
    internal class ResetPasswordCommandValidation : CustomAbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Inavalid Email");
            RuleFor(x => x.Password)
                .Matches("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]){8,30}$")
                .WithMessage("Inavalid Password");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required");
        }
    }
}
