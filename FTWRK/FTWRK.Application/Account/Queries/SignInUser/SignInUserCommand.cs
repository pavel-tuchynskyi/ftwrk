using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Queries.SignInUser
{
    public class SignInUserCommand : IRequest<Token>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
