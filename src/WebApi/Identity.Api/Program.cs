using Engineering.Infrastructure;
using Identity.Api.Authentication;
using Identity.Api.Middleware;
using Identity.Application;
using Identity.Infrastructure;
using System.Text.Json.Serialization;
using Engineering.Application;
using Country.Infrastructure;
using Country.Application;
using Country.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CourierJob.Infrastructure.Persistence.Context;
using City.Application;
using CarrierJob.Infrastructure;
using CarrierJob.Application;
using ResultCurriers.Infrastructure;
using SharedKernel.Abstractions.Encript;
using SharedKernel.Helpers;
using Couriers.Application;
using Couriers.Infrastructure;
using Couriers.Infrastructure.Persistence.Repositories;
using Couriers.Application.Interfaces;
using ResultCouriers.Application.Interfaces;
using ResultCouriers.Infrastructure;
using ResultCouriers.Application;
using Couriers.Infrastructure.Persistence.Context;
using ResultCouriers.Infrastructure.Persistence.Context;


// --- Constructor de la aplicación (Application Builder) ---
var builder = WebApplication.CreateBuilder(args);
var Cors = "Cors";

// --- Registro de servicios (Service Registration) ---
// Agrega servicios al contenedor.
builder.Services
    .AddInfrastructureIdentity(builder.Configuration)
    .AddApplicationIdentity()
    .AddInfrastructureEngineering(builder.Configuration)
    .AddApplicationEngineering()
    .AddInfrastructureCountry(builder.Configuration)
    .AddApplicationCountry()
    .AddInfrastructureCity(builder.Configuration)
    .AddApplicationCity()
    .AddInfrastructureCarrierJob(builder.Configuration)
    .AddInfrastructureResultCurriers(builder.Configuration)
    .AddApplicationCouriersJob()
    .AddApplicationCouriers()
    .AddApplicationResultCouriers()
    .AddInfrastructureCouriers(builder.Configuration)
    .AddAuthentication(builder.Configuration);
    
builder.Services.AddScoped<ICourierRepository, Couriers.Infrastructure.Persistence.Repositories.CouriesRepository> ();

// Registra el DbContext con la cadena de conexión
builder.Services.AddDbContext<CountryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<CountryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<CourierJobDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<ResultCouriersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddDbContext<CouriersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
       
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

       // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddScoped<
    ResultCouriers.Application.Interfaces.IResultCourierRPA,
    ResultCouriers.Infrastructure.ResultCourierRPA>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
        builder =>
        {
            builder.WithOrigins("*");
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});
string aesKeyBase64 = builder.Configuration
    .GetSection("EncryptionSettings:KeyBase64").Value
    ?? throw new InvalidOperationException("La clave AES no está configurada.");

builder.Services.AddSingleton<IDataEncryptor>(provider =>
    new AesEncryptor(aesKeyBase64)
);
builder.Services.AddHttpClient<ResultCouriers.Application.Interfaces.IResultCourierRPA, ResultCouriers.Infrastructure.ResultCourierRPA>(client =>
{
  

    client.Timeout = TimeSpan.FromSeconds(3600);
});


var app = builder.Build();


// --- Middleware y configuración (Middleware and Configuration) ---
// Ejecuta las migraciones de la base de datos al iniciar.
// Nota: Esta es una buena práctica para el desarrollo, pero para entornos de producción con múltiples instancias,
// es mejor aplicar las migraciones como un paso separado en tu canal de despliegue.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CountryDbContext>();
    db.Database.Migrate();
}

app.UseCors(Cors);

// Configura el pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.AddMiddleware();

app.MapControllers();

// --- Ejecuta la aplicación ---
app.Run();