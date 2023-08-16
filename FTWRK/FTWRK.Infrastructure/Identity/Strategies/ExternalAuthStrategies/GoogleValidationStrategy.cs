using FTWRK.Application.Common.Exceptions;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Interfaces;
using Google.Apis.Auth;
using Serilog;

namespace FTWRK.Infrastructure.Identity.Strategies.ExternalAuthStrategies
{
    public class GoogleValidationStrategy : IExternalAuth
    {
        public readonly ExternalProviders authType = ExternalProviders.GOOGLE;
        private readonly GoogleAuthConfig _authConfig;

        public GoogleValidationStrategy(ExternalAuthConfiguration authConfiguration, IHttpClientFactory httpClientFactory)
        {
            _authConfig = authConfiguration.GoogleAuthConfig;
        }

        public async Task<ExternalAuthUserResult> ValidateTokenAsync(string token)
        {
            var payload = await VerifyGoogleTokenAsync(token);

            return new ExternalAuthUserResult(payload.GivenName, payload.Email);
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _authConfig.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

            if (payload == null)
            {
                Log.Error("Invalid token");
                throw new BadRequestException("Invalid token");
            }

            return payload;
        }
    }
}
