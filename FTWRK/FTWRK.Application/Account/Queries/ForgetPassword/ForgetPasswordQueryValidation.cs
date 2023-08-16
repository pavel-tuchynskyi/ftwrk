using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Queries.ForgetPassword
{
    public class ForgetPasswordQueryValidation : CustomAbstractValidator<ForgetPasswordQuery>
    {
        public ForgetPasswordQueryValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Email");
            RuleFor(x => x.Link).NotEmpty().WithMessage("Confirmation Action is required");
        }
    }
}
