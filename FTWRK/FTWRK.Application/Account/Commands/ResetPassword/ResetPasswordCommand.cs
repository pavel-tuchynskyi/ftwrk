using MediatR;

namespace FTWRK.Application.Account.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
