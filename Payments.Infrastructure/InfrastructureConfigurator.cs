using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Infrastructure.Repositories;
using Payments.Infrastructure.Repositories.Interfaces;

namespace Payments.Infrastructure
{
    public static class InfrastructureConfigurator
    {
        public static IServiceCollection InfrastructureConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PaymentsConnection")));

            // Repositories
            services.AddScoped<IApprovedAuthorizationRepository, ApprovedAuthorizationRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
