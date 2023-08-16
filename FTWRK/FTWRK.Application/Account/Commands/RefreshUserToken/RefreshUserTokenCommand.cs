using FTWRK.Application.Common.Models;
using MediatR;

namespace FTWRK.Application.Account.Commands.RefreshUserToken
{
    public class RefreshUserTokenCommand : Token, IRequest<Token>
    {
    }
}
