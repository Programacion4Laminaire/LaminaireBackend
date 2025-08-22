using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("UserId");

        builder.Property(e => e.Identification)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(e => e.Identification)
            .IsUnique();

        builder.Property(e => e.BirthDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(e => e.FirstName)
            .HasMaxLength(80);

        builder.Property(e => e.LastName)
            .HasMaxLength(100);

        builder.HasIndex(e => e.UserName)
            .IsUnique();
        builder.Property(e => e.UserName)
            .HasMaxLength(70);

        builder.HasIndex(e => e.Email)
            .IsUnique();
        builder.Property(e => e.Email)
            .HasMaxLength(200);

        builder.Property(e => e.ProfileImagePath)
    .HasMaxLength(250);
    }
}
