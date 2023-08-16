using MediatR;

namespace FTWRK.Application.Account.Queries.ForgetPassword
{
    public class ForgetPasswordQuery : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Link { get; set; }
    }
}
