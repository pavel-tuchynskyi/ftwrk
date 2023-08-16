using FluentValidation;

namespace FTWRK.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandValidation : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("RoleName is reqired");
        }
    }
}
