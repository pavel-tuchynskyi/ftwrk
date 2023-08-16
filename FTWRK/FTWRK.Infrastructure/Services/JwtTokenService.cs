using FTWRK.Application.Common.Exceptions;
using FTWRK.Infrastructure.Configuration;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FTWRK.Infrastructure.Identity
{
    public class JwtTokenService : ITokenService
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public JwtTokenService(IOptions<TokenConfiguration> tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration.Value;
        }

        public string CreateAccessToken(ApplicationUser user, List<string> roles)
        {
            var creds = GetSignInCredentials();
            var claims = GetClaims(user, roles);
            var tokenOptions = GenerateTokenOptions(creds, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials creds, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
                (
                    issuer: _tokenConfiguration.Issuer,
                    audience: _tokenConfiguration.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(2),
                    signingCredentials: creds
                );

            return tokenOptions;
        }

        private List<Claim> GetClaims(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SigningCredentials GetSignInCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_tokenConfiguration.Key);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public UserRefreshToken CreateRefreshToken()
        {
            var refreshToken = new UserRefreshToken()
            {
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(2)
            };

            return refreshToken;
        }

        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null)
            {
                Log.Error("Token is invalid");
                throw new BadRequestException("Invalid token");
            }

            return claims;
        }

        public bool ValidateRefreshTokenLifetime(string tokenToValidate, UserRefreshToken sourceToken)
        {
            if (sourceToken.RefreshToken != tokenToValidate)
            {
                Log.Error("Token is invalid: {token}", tokenToValidate);
                throw new BadRequestException("Invalid token");
            }

            return true;
        }

        public (string AccessToken, UserRefreshToken RefreshToken) CreateTokens(ApplicationUser user, List<string> roles)
        {
            var token = CreateAccessToken(user, roles);
            var refreshToken = CreateRefreshToken();

            return (token, refreshToken);
        }
    }
}
