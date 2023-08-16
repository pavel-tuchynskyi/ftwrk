using FTWRK.Application.Common.Interfaces;
using FTWRK.Infrastructure.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FTWRK.Functions.Startup))]
namespace FTWRK.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IAudioConverter, AudioConverter>();
        }
    }
}
