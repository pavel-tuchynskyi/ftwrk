using MediatR;

namespace FTWRK.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Unit>
    {
        public string RoleName { get; set; }
    }
}
