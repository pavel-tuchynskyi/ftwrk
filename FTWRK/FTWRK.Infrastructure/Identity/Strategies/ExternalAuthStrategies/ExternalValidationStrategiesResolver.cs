using FTWRK.Application.Common.Helpers;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Interfaces;

namespace FTWRK.Infrastructure.Identity.Strategies.ExternalAuthStrategies
{
    public class ExternalValidationStrategiesResolver
    {
        private readonly ExternalAuthConfiguration _authConfiguration;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalValidationStrategiesResolver(ExternalAuthConfiguration authConfiguration, IHttpClientFactory httpClientFactory)
        {
            _authConfiguration = authConfiguration;
            _httpClientFactory = httpClientFactory;
        }

        public IExternalAuth GetExternalAuthStrategy(string provider)
        {
            var authType = (ExternalProviders)Enum.Parse(typeof(ExternalProviders), provider);
            var authStrategies = StrategyHelper.GetStrategies<ExternalProviders, IExternalAuth>(typeof(ExternalValidationStrategiesResolver), _authConfiguration, _httpClientFactory);

            return authStrategies[authType];
        }
    }
}
