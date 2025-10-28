using City.Domain.Entities;
using Country.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourierJob.Infrastructure.Persistence.Context;

    public class CourierJobDbContext : DbContext
    {
        public CourierJobDbContext(DbContextOptions<CourierJobDbContext> options) : base(options) { }

     
        public DbSet<SharedKernel.Domain.Entities.CourierJob> CourierJobs { get; set; }
         public DbSet<SharedKernel.Domain.Entities.Couriers> Couriers { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SharedKernel.Domain.Entities.Couriers>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Couries");
        });
        modelBuilder.Entity<SharedKernel.Domain.Entities.ResultCouriers>(entity =>
        {
         
            entity.ToTable("CourierResults");

            entity.Property(r => r.IdCourier)
          .HasColumnName("IdCourier");

            entity.HasOne(r => r.CourierJob)
                  .WithMany(j => j.CourierResults) 
                  .HasForeignKey(r => r.IdCourierJob);
            
            entity.HasOne(r => r.Couriers)
                  .WithMany(c => c.CourierResults) 
                  .HasForeignKey(r => r.IdCourier) 
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasKey(t => t.Id);

        });

      
        modelBuilder.Entity<CityEntity>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.ToTable("Cities");
            });

        // Configuración de la entidad Country
        modelBuilder.Entity<CountryEntity>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.ToTable("Countries");
        });

        modelBuilder.Entity<SharedKernel.Domain.Entities.CourierJob>(entity =>
            {
                entity.HasKey(j => j.Id);

                entity.ToTable("CourierJobs");

                entity.HasOne(j => j.originCity)
                      .WithMany()
                      .HasForeignKey(j => j.OriginCityId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(j => j.destinationCity)
                      .WithMany()
                      .HasForeignKey(j => j.DestinationCityId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(j => j.originCountry)
                     .WithMany()
                     .HasForeignKey(j => j.OriginCountryId)
                     .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(j => j.destinationCountry)
                   .WithMany()
                   .HasForeignKey(j => j.DestinationCountryId)
                   .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }


