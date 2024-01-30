using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Options;
using Payments.Application.Services;
using Payments.Application.Services.Interfaces;

namespace Payments.Application
{
    public static class ApplicationConfigurator
    {
        public static IServiceCollection ApplicationConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurations
            services.Configure<ExternalServiceOptions>(configuration.GetSection(ExternalServiceOptions.External));

            // Mappers
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Services
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
