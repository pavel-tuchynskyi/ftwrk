using FTWRK.Application.Common.Interfaces;
using FTWRK.Infrastructure.Configuration.ExternalAuth;
using FTWRK.Infrastructure.Idenity.Models;
using FTWRK.Infrastructure.Identity;
using FTWRK.Infrastructure.Interfaces;
using FTWRK.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FTWRK.Infrastructure.Configuration
{
    public static class InfrastructureConfiguration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection(nameof(MongoOptions)));
            services.Configure<TokenConfiguration>(configuration.GetSection(nameof(TokenConfiguration)));
            services.Configure<ExternalAuthConfiguration>(configuration.GetSection(nameof(ExternalAuthConfiguration)));
            services.Configure<SmtpConfiguration>(configuration.GetSection(nameof(SmtpConfiguration)));

            services.AddMongoIdentity(configuration);

            services.AddAuthentication(configuration);

            services.AddScoped<ISignInManager, SignInManagerService>();
            services.AddScoped<IUserManager, UserManagerService>();
            services.AddScoped<IRoleManager, RoleManagerService>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAudioService, AudioService>();
        }

        private static void AddMongoIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>();
            
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = string.Empty;
            })
                .AddRoles<ApplicationUserRole>()
                .AddDefaultTokenProviders()
                .AddMongoDbStores<ApplicationUser, ApplicationUserRole, Guid>(options.ConnectionString, options.DatabaseName);
        }

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(TokenConfiguration)).Get<TokenConfiguration>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });
        }
    }
}
