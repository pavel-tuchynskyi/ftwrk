using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FTWRK.Application.Configuration
{
    public static class ConfigureServices
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
