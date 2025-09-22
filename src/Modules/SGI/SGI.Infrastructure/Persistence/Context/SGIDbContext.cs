using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGI.Domain.Entities;
using System.Data;
using System.Reflection;

namespace SGI.Infrastructure.Persistence.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public DbSet<Consumption> Users { get; set; } = null!;
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public IDbConnection CreateConnection() => new SqlConnection(_configuration.GetConnectionString("SGIConnection"));
}
