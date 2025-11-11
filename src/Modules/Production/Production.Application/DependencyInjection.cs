using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Behaviours;
using System.Reflection;

namespace Production.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationProduction(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<HandlerExecutor>();
        services.AddScoped<IValidationService, ValidationService>();

        services.AddHandlersFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IDispatcher, Dispatcher>();

        return services;
    }

    private static void AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                     i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))));

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                     i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handlerInterface in interfaces)
            {
                services.AddScoped(handlerInterface, handlerType);
            }
        }
    }
}
