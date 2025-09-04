using Engineering.Application.Interfaces.Persistence;
using Engineering.Application.Interfaces.Services;
using Engineering.Infrastructure.Persistence.Context;
using Engineering.Infrastructure.Persistence.Repositories;
using Engineering.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Engineering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEngineering(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Registro del DbContext para Dapper
        services.AddSingleton<EngineeringDbContext>();

        // Registro automático de repositorios (igual que en Identity)
        var infraAsm = Assembly.GetExecutingAssembly();
        foreach (var impl in infraAsm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
        {
            var @interface = impl.GetInterfaces()
                .FirstOrDefault(i => i.Name == "I" + impl.Name);
            if (@interface != null)
                services.AddScoped(@interface, impl);
        }

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
