using FluentValidation;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Account.Commands.AddUserToRole
{
    public class AddUserToRoleCommandValidation : CustomAbstractValidator<AddUserToRoleCommand>
    {
        public AddUserToRoleCommandValidation()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("RoleName is required");
        }
    }
}
