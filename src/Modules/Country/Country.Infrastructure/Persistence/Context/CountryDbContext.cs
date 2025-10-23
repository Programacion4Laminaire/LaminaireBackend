namespace Country.Infrastructure.Persistence.Context;
using Country.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CountryDbContext : DbContext
{
    public CountryDbContext(DbContextOptions<CountryDbContext> options)
        : base(options)
    {
    }

    public DbSet<CountryEntity> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aquí puedes configurar tu modelo de datos, como claves primarias, índices, etc.
        modelBuilder.Entity<CountryEntity>().HasKey(c => c.Id);

        // Configuración de la tabla, si es necesario
        modelBuilder.Entity<CountryEntity>().ToTable("Countries");
    }
}
