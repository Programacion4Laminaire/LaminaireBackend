using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGI.Application.Interfaces.Persistence;
using SGI.Application.Interfaces.Services;
using SGI.Infrastructure.Persistence.Context;
using SGI.Infrastructure.Persistence.Repositories;
using SGI.Infrastructure.Services;
using SharedKernel.Abstractions.Services;
using System.Reflection;

namespace SGI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureSGI(this IServiceCollection services, ConfigurationManager configuration)
    {
     
       

        var assembly = typeof(ApplicationDbContext).Assembly.FullName;

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseSqlServer(configuration.GetConnectionString("SGIConnection"),
                              b => b.MigrationsAssembly(assembly)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Registro automático de repositorios EF
        var infraAsm = Assembly.GetExecutingAssembly();
        foreach (var impl in infraAsm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
        {
            var @interface = impl.GetInterfaces()
                .FirstOrDefault(i => i.Name == "I" + impl.Name);
            if (@interface != null)
                services.AddScoped(@interface, impl);
        }

        // Repositorios/Servicios Dapper
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IOrderingQuery, OrderingQuery>();
        services.AddScoped<IConsumptionExportService, ConsumptionExportService>();


        return services;
    }
}