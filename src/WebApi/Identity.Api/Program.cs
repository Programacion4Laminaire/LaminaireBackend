using Engineering.Infrastructure;
using Identity.Api.Authentication;
using Identity.Api.Middleware;
using Identity.Application;
using Identity.Infrastructure;
using System.Text.Json.Serialization;
using Engineering.Application;
var builder = WebApplication.CreateBuilder(args);
var Cors = "Cors";
// Add services to the container.
builder.Services
    .AddInfrastructureIdentity(builder.Configuration)
    .AddApplicationIdentity()
    .AddInfrastructureEngineering(builder.Configuration)
    .AddApplicationEngineering()
    .AddAuthentication(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var app = builder.Build();

app.UseCors(Cors);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.AddMiddleware();

app.MapControllers();

app.Run();
