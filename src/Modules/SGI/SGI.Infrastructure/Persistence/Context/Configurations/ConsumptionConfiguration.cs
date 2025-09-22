using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGI.Domain.Entities;

namespace SGI.Infrastructure.Persistence.Context.Configurations;

internal sealed class ConsumptionConfiguration : IEntityTypeConfiguration<Consumption>
{
    public void Configure(EntityTypeBuilder<Consumption> b)
    {
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).HasColumnName("ConsumptionId");

        b.Property(e => e.ResourceType)
            .IsRequired()
            .HasMaxLength(16); // ENERGY/GAS/WATER

        b.Property(e => e.Year).IsRequired();
        b.Property(e => e.Month).IsRequired();

        b.Property(e => e.Value).HasPrecision(18, 4).IsRequired();
        b.Property(e => e.Unit).HasMaxLength(16).IsRequired();

        b.Property(e => e.ReadingDate).HasColumnType("date");
        b.Property(e => e.MeterCode).HasMaxLength(64);
        b.Property(e => e.Note).HasMaxLength(500);

        // Evita duplicados por recurso-año-mes
        b.HasIndex(e => new { e.ResourceType, e.Year, e.Month })
         .IsUnique();

        // Filtros suaves (ya los maneja GenericRepository con AuditDelete null)
    }
}