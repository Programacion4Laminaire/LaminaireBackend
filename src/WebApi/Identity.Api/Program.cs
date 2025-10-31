using Engineering.Application;
using Engineering.Infrastructure;
using Identity.Api.Authentication;
using Identity.Api.Middleware;
using Identity.Api.Services;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.RealTime;
using Logistics.Application;
using Logistics.Infrastructure;
using SGI.Application;
using SGI.Infrastructure;
using SharedKernel.Abstractions.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
const string Cors = "Cors";

builder.Services
    .AddInfrastructureIdentity(builder.Configuration)
    .AddApplicationIdentity()
    .AddInfrastructureEngineering(builder.Configuration)
    .AddApplicationEngineering()
    .AddApplicationSGI()
    .AddInfrastructureSGI(builder.Configuration)
    .AddInfrastructureLogistics(builder.Configuration)
    .AddApplicationLogistics()
    .AddAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(Cors, p =>
        p.AllowAnyOrigin()        // <-- WithOrigins("*") no es válido
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors(Cors);

// --- Swagger: IMPORTANTÍSIMO EN SUB-APP ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";                 // UI en /SirBackend/swagger
    c.SwaggerEndpoint("v1/swagger.json",       // relativo, SIN "/" inicial
                      "SirBackend v1");
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.AddMiddleware();

app.MapControllers();
app.MapHub<PermissionsHub>("/hubs/permissions");
app.Run();