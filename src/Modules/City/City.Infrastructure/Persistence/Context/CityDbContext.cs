
using City.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace City.Infrastructure.Persistence.Context;
public class CityDbContext : DbContext
{
    public CityDbContext(DbContextOptions<CityDbContext> options)
        : base(options)
    {
    }
    public CityDbContext() { }

    public DbSet<CityEntity> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Country
        modelBuilder.Entity<CityEntity>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Cities");

            entity.HasOne(c => c.Country)
                  .WithMany()
                  .HasForeignKey(c => c.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Metadata
                  .GetForeignKeys()
                  .First(fk => fk.PrincipalEntityType.ClrType == typeof(Country.Domain.Entities.CountryEntity))
                  .PrincipalEntityType.SetTableName("Countries");
        });
    }
}
