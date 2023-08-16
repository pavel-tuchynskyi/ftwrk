using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.RegisterUser
{
    public class CreateUserCommand : IRequest<Token>
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
