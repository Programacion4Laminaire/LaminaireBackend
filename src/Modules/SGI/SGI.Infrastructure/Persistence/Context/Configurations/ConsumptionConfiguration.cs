using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGI.Domain.Entities;

namespace SGI.Infrastructure.Persistence.Context.Configurations;

internal sealed class ConsumptionConfiguration : IEntityTypeConfiguration<Consumption>
{
    public void Configure(EntityTypeBuilder<Consumption> builder)
    {
        builder.ToTable("Consumptions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("ConsumptionId");

        builder.Property(e => e.ResourceType)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        builder.Property(e => e.Unit)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(e => e.ReadingDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(e => e.Note)
            .HasMaxLength(500);

        
        builder.Ignore(e => e.State);

        
        builder.HasIndex(e => new { e.ResourceType, e.ReadingDate })
               .IsUnique()
               .HasDatabaseName("UX_Consumptions_ResourceType_ReadingDate");
    }
}
