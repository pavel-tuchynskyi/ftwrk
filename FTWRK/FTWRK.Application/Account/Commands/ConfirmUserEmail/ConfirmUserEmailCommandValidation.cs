using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.ConfirmUserEmail
{
    public class ConfirmUserEmailCommandValidation : CustomAbstractValidator<ConfirmUserEmailCommand>
    {
        public ConfirmUserEmailCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required");
        }
    }
}
