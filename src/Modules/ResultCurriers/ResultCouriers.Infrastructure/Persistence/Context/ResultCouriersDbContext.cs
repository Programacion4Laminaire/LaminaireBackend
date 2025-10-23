using Microsoft.EntityFrameworkCore;

namespace ResultCouriers.Infrastructure.Persistence.Context;


public class ResultCouriersDbContext : DbContext
{
    public ResultCouriersDbContext(DbContextOptions<ResultCouriersDbContext> options) : base(options) { }

   
    public DbSet<SharedKernel.Domain.Entities.ResultCouriers> Results { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SharedKernel.Domain.Entities.CourierJob>(entity =>
        {
          
            entity.ToTable("CourierJobs");

         
            entity.HasKey(t => t.Id);

        
        });
        modelBuilder.Entity<City.Domain.Entities.CityEntity>(entity =>
        {

            entity.ToTable("Cities");


            entity.HasKey(t => t.Id);


        });
        modelBuilder.Entity<Country.Domain.Entities.CountryEntity>(entity =>
        {

            entity.ToTable("Countries");


            entity.HasKey(t => t.Id);


        });
        modelBuilder.Entity<SharedKernel.Domain.Entities.Couriers>(entity =>
        {

            entity.ToTable("Couries");


            entity.HasKey(t => t.Id);


        });
        modelBuilder.Entity<SharedKernel.Domain.Entities.ResultCouriers>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.ToTable("CourierResults");

            entity.Property(r => r.IdCourier)
            .HasColumnName("IdCourier");

            entity.Property(t => t.Price)
                .HasColumnType("decimal(18, 3)")
                .IsRequired();

            entity.Property(t => t.Currency)
                .HasMaxLength(5)
                .IsRequired();

           
            entity.Property(t => t.Service)
                .HasMaxLength(150)
                .IsRequired();

            entity.HasOne(r => r.CourierJob)
             .WithMany(j => j.CourierResults)
             .HasForeignKey(r => r.IdCourierJob)
             .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Couriers)
                  .WithMany(c => c.CourierResults)
                  .HasForeignKey(r => r.IdCourier)
                  .OnDelete(DeleteBehavior.Restrict);


        });

    }
}