using AspNetCore.CacheOutput.InMemory.Extensions;
using FTWRK.Application.Common.Interfaces;
using FTWRK.Web.Models;
using FTWRK.Web.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace FTWRK.Web.Configuration
{
    public static class ConfigureServices
    {
        public static void ConfigureWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureCors();

            services.AddHttpContextAccessor();

            services.AddInMemoryCacheOutput();

            services.Configure<ClientURIData>(configuration.GetSection(nameof(ClientURIData)));

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            services.AddScoped<IUserContextService, UserContextService>();
        }

        private static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FtwrkCors", pol =>
                {
                    pol.AllowAnyOrigin();
                    pol.AllowAnyHeader();
                    pol.AllowAnyMethod();
                });
            });
        }
    }
}
