using FTWRK.Application.Common.Interfaces;
using MediatR;

namespace FTWRK.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Unit>
    {
        private readonly IRoleManager _roleManager;

        public CreateRoleCommandHandler(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            await _roleManager.CreateRoleAsync(request.RoleName);

            return Unit.Value;
        }
    }
}
