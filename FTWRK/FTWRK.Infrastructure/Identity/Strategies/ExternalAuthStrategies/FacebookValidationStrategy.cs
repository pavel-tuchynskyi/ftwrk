using FTWRK.Application.Common.Exceptions;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Interfaces;
using Newtonsoft.Json;
using Serilog;

namespace FTWRK.Infrastructure.Identity.Strategies.ExternalAuthStrategies
{
    public class FacebookValidationStrategy : IExternalAuth
    {
        public readonly ExternalProviders _authType = ExternalProviders.FACEBOOK;
        private const string TokenValidationUrl = "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";
        private const string UserInfoUrl = "https://graph.facebook.com/v10.0/me?access_token={0}&fields=name%2Cemail%2Cpicture%2Cfirst_name%2Clast_name&method=get&pretty=0&sdk=joey&suppress_http_code=1";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FacebookAuthConfig _authConfig;

        public FacebookValidationStrategy(ExternalAuthConfiguration authConfiguration, IHttpClientFactory httpClientFactory)
        {
            _authConfig = authConfiguration.FacebookAuthConfig;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ExternalAuthUserResult> ValidateTokenAsync(string token)
        {
            var validationResult = await VarifyFacebookTokenAsync(token);

            if (!validationResult.Data.IsValid)
            {
                Log.Error("Invalid token");
                throw new BadRequestException("Invalid token");
            }

            var userResult = await GetUserDataAsync(token);

            return userResult;
        }

        private async Task<FacebookValidationResult> VarifyFacebookTokenAsync(string token)
        {
            var formatterUrl = string.Format(TokenValidationUrl, token, _authConfig.AppId, _authConfig.Secret);
            var result = await _httpClientFactory.CreateClient().GetAsync(formatterUrl);

            result.EnsureSuccessStatusCode();
            var responseString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<FacebookValidationResult>(responseString);
        }

        private async Task<ExternalAuthUserResult> GetUserDataAsync(string token)
        {
            var formatterUrl = string.Format(UserInfoUrl, token);
            var result = await _httpClientFactory.CreateClient().GetAsync(formatterUrl);

            result.EnsureSuccessStatusCode();
            var responseString = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ExternalAuthUserResult>(responseString);
        }
    }
}
