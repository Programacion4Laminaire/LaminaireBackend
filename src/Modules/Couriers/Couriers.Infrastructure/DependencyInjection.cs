using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Couriers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureCouriers(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Registro del DbContext con SQL Server
        services.AddDbContext<Couriers.Infrastructure.Persistence.Context.CouriersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        // Registro automático de repositorios
        var infraAsm = Assembly.GetExecutingAssembly();
        foreach (var impl in infraAsm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
        {
            var @interface = impl.GetInterfaces()
                .FirstOrDefault(i => i.Name == "I" + impl.Name);
            if (@interface != null)
                services.AddScoped(@interface, impl);
        }

        return services;
    }
}
