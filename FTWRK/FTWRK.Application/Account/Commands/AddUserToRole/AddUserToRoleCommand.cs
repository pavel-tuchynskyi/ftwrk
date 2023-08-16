using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.AddUserToRole
{
    public class AddUserToRoleCommand : IRequest<Token>
    {
        public Token Token { get; set; }
        public string RoleName { get; set; }
    }
}
