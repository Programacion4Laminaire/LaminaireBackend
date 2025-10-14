using Logistics.Application.Interfaces.Persistence;
using Logistics.Application.Interfaces.Services;
using Logistics.Infrastructure.Persistence.Context;
using Logistics.Infrastructure.Persistence.Repositories;
using Logistics.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logistics.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLogistics(this IServiceCollection services, ConfigurationManager configuration)
    {
        // DbContext para Dapper
        services.AddSingleton<LogisticsDbContext>();

        // Repositorios
        services.AddScoped<IAccessoryEquivalenceRepository, AccessoryEquivalenceRepository>();

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
