using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Production.Application.Interfaces.Persistence;
using Production.Application.Interfaces.Services;
using Production.Infrastructure.Persistence.Context;
using Production.Infrastructure.Persistence.Repositories;
using Production.Infrastructure.Services;

namespace Production.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureProduction(this IServiceCollection services, ConfigurationManager configuration)
        {
            // DbContext para Dapper
            services.AddSingleton<ProductionDbContext>();

            // Repositorios
            services.AddScoped<IReprogramLinesRepository, ReprogramLinesRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
