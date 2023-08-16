using MediatR;

namespace FTWRK.Application.Account.Commands.ConfirmUserEmail
{
    public class ConfirmUserEmailCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
