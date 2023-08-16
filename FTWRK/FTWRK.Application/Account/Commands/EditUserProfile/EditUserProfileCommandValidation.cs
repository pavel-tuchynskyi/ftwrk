using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.EditUserProfile
{
    public class EditUserProfileCommandValidation : CustomAbstractValidator<EditUserProfileCommand>
    {
        public EditUserProfileCommandValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Invalid FullName");
            RuleFor(x => x.UserName).MinimumLength(5).MaximumLength(30).WithMessage("Invalid UserName");
        }
    }
}
