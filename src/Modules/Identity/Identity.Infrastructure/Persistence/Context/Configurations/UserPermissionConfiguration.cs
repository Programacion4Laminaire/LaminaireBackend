
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.Ignore(x => x.Id);
        builder.HasKey(e => new { e.UserId, e.PermissionId });

        builder.Property(e => e.IsGranted).IsRequired();

        // Relaciones (ajusta nombres si tus entidades difieren)
        builder.HasOne(up => up.User)
               .WithMany() // o .WithMany(u => u.UserPermissions) si lo tienes
               .HasForeignKey(up => up.UserId);

        builder.HasOne(up => up.Permission)
               .WithMany()
               .HasForeignKey(up => up.PermissionId);
    }
}
