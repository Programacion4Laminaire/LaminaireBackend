using Microsoft.EntityFrameworkCore;

namespace Couriers.Infrastructure.Persistence.Context;


public class CouriersDbContext : DbContext
{
    public CouriersDbContext(DbContextOptions<CouriersDbContext> options) : base(options) { }


    public DbSet<SharedKernel.Domain.Entities.Couriers> Couriers { get; set; } = null!;
   // public IEnumerable<object> Couriers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

     
        modelBuilder.Entity<SharedKernel.Domain.Entities.Couriers>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.ToTable("Couries");


            entity.Property(t => t.Password)
                .HasMaxLength(150);




        });

    }
}