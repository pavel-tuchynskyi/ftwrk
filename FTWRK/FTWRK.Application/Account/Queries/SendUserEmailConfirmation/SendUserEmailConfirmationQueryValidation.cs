using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Queries.SendUserEmailConfirmation
{
    public class SendUserEmailConfirmationQueryValidation : CustomAbstractValidator<SendUserEmailConfirmationQuery>
    {
        public SendUserEmailConfirmationQueryValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Link).NotEmpty().WithMessage("Confirmation Action is required");
        }
    }
}
