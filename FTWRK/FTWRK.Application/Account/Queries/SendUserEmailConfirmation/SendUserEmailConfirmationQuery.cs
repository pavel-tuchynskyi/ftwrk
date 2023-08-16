using MediatR;

namespace FTWRK.Application.Account.Queries.SendUserEmailConfirmation
{
    public class SendUserEmailConfirmationQuery : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Link { get; set; }
    }
}
