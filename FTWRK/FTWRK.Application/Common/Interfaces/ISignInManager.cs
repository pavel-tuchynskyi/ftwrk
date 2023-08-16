using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Models;

namespace FTWRK.Application.Common.Interfaces
{
    public interface ISignInManager
    {
        Task<Token> SignInUserAsync(string email, string passwod);
        Task<Token> RefreshUserToken(Token token);
        Task<Token> ExternalUserSignInAsync(ExternalSignInUserDto userDto);
    }
}
