using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.RegisterUser
{
    public class CreateUserCommandValidation : CustomAbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(5).MaximumLength(30).WithMessage("Invalid user name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(x => x.Password).NotEmpty()
                .Matches("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]){8,30}$")
                .WithMessage("Invalid id");
            RuleFor(x => x.Age).GreaterThan(5).LessThan(100).WithMessage("Invalid age");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Invalid country");
        }
    }
}
