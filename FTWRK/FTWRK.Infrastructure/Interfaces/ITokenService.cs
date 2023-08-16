using FTWRK.Infrastructure.Idenity.Models;
using System.Security.Claims;

namespace FTWRK.Infrastructure.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(ApplicationUser user, List<string> roles);
        UserRefreshToken CreateRefreshToken();
        ClaimsPrincipal GetClaimsFromToken(string token);
        bool ValidateRefreshTokenLifetime(string tokenToValidate, UserRefreshToken sourceToken);
        (string AccessToken, UserRefreshToken RefreshToken) CreateTokens(ApplicationUser user, List<string> roles);
    }
}
