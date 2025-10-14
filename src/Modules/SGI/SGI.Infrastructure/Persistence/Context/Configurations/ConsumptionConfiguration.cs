using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGI.Domain.Entities;

namespace SGI.Infrastructure.Persistence.Context.Configurations
{
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

            // 👇 Nuevo campo Sede
            builder.Property(e => e.Sede)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.DailyConsumption)
    .HasColumnType("decimal(18,4)")
    .IsRequired(false);

            builder.Ignore(e => e.State);

            // ✅ Índice único correcto: incluye Sede, ResourceType y ReadingDate
            builder.HasIndex(e => new { e.Sede, e.ResourceType, e.ReadingDate })
                .IsUnique()
                .HasFilter("[AuditDeleteDate] IS NULL")
                .HasDatabaseName("UX_Consumptions_Sede_ResourceType_ReadingDate");
        }
    }
}
