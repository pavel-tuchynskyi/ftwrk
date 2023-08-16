using FTWRK.Application.Common.DTO.Account;
using FTWRK.Application.Common.Exceptions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Extensions;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Identity.Strategies.ExternalAuthStrategies;
using FTWRK.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;

namespace FTWRK.Infrastructure.Identity
{
    public class SignInManagerService : ISignInManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ExternalAuthConfiguration _authConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public SignInManagerService(UserManager<ApplicationUser> userManager, ITokenService tokenService,
            IOptions<ExternalAuthConfiguration> authConfiguration, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _authConfiguration = authConfiguration.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Token> SignInUserAsync(string email, string passwod)
        {
            var userData = await GetUserDataAsync(email);

            await CheckUserPasswordAsync(userData.User, passwod);

            var tokenResult = await CreateTokenResult(userData.User, userData.Roles);

            return tokenResult;
        }

        private async Task<(ApplicationUser User, List<string> Roles)> GetUserDataAsync(string userEmail)
        {
            var user = await _userManager.FindAppUserByEmailAsync(userEmail);

            var roles = await _userManager.GetRolesAsync(user);

            return (user, roles.ToList());
        }

        private async Task<bool> CheckUserPasswordAsync(ApplicationUser user, string password)
        {
            var passwordResult = await _userManager.CheckPasswordAsync(user, password);

            if (!passwordResult)
            {
                Log.Error("Wrong password for user: {id}", user.Id);
                throw new NotFoundException("Wrong user password");
            }

            return true;
        }

        public async Task<Token> RefreshUserToken(Token token)
        {
            var claims = _tokenService.GetClaimsFromToken(token.AccessToken);
            var userData = await GetUserDataAsync(claims.FindFirst(ClaimTypes.Email).Value);

            _tokenService.ValidateRefreshTokenLifetime(token.RefreshToken, userData.User.RefreshToken);

            var tokenResult = await CreateTokenResult(userData.User, userData.Roles);

            return tokenResult;
        }

        private async Task<Token> CreateTokenResult(ApplicationUser user, List<string> roles)
        {
            var tokens = _tokenService.CreateTokens(user, roles);
            user.RefreshToken = tokens.RefreshToken;
            await _userManager.UpdateAppUserAsync(user);

            return new Token
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken.RefreshToken
            };
        }

        public async Task<Token> ExternalUserSignInAsync(ExternalSignInUserDto signInInfo)
        {
            var resolver = new ExternalValidationStrategiesResolver(_authConfiguration, _httpClientFactory);
            var authStrategy = resolver.GetExternalAuthStrategy(signInInfo.Provider);
            var result = await authStrategy.ValidateTokenAsync(signInInfo.Token);

            var user = await _userManager.GetOrCreateUser(result.Email, result.UserName);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenResult = await CreateTokenResult(user, roles.ToList());

            return tokenResult;
        }
    }
}
