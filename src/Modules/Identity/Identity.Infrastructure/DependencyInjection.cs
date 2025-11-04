using Identity.Application.Interfaces.Authentication;
using Identity.Application.Interfaces.Persistence;
using Identity.Application.Interfaces.RealTime;
using Identity.Application.Interfaces.Services;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Persistence.Context;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.RealTime;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions.Services;
using System.Reflection;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services, ConfigurationManager configuration)
    {
        // DbContext Dapper
        services.AddScoped<LaminaireDbContext>();

        var assembly = typeof(ApplicationDbContext).Assembly.FullName;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                                 b => b.MigrationsAssembly(assembly)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Registro automático de repositorios EF
        var infraAsm = Assembly.GetExecutingAssembly();
        foreach (var impl in infraAsm.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
        {
            var @interface = impl.GetInterfaces().FirstOrDefault(i => i.Name == "I" + impl.Name);
            if (@interface != null) services.AddScoped(@interface, impl);
        }

        // Dapper repos y servicios
        services.AddScoped<IUserCoockiesRepository, UserCoockiesRepository>();
        services.AddScoped<ILaminaireUserRepository, LaminaireUserRepository>();
        services.AddScoped<IUserCookieService, UserCookieService>();

        // Infraestructura base
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IOrderingQuery, OrderingQuery>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        // Auth
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // 👇 lifetimes correctos
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Repos usados por PermissionService / handlers
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();

        // SignalR
        services.AddSignalR();

        // Notificador de permisos
        services.AddScoped<IPermissionsNotifier, PermissionsNotifier>();

        return services;
    }
}
