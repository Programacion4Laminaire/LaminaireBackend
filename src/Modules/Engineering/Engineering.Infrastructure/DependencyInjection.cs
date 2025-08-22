using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions.Services;

namespace Engineering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            //var assembly = typeof(ApplicationDbContext).Assembly.FullName;

            //services.AddDbContext<ApplicationDbContext>(
            //    options => options
            //    .UseSqlServer(configuration.GetConnectionString("EngineeringConnection"), b => b.MigrationsAssembly(assembly)));

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //var infraAsm = Assembly.GetExecutingAssembly();
            //foreach (var impl in infraAsm.GetTypes()
            //             .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository")))
            //{
            //    var @interface = impl.GetInterfaces()
            //        .FirstOrDefault(i => i.Name == "I" + impl.Name);
            //    if (@interface != null)
            //        services.AddScoped(@interface, impl);
            //}

            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IOrderingQuery, OrderingQuery>();



            return services;
        }
    }
}
