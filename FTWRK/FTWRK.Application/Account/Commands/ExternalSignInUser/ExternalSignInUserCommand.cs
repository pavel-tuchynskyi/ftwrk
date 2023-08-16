using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.ExternalSignInUser
{
    public class ExternalSignInUserCommand : IRequest<Token>
    {
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
