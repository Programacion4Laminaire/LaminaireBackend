// Identity.Infrastructure/Persistence/Context/Configurations/MenuRoleConfiguration.cs
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Context.Configurations;

internal sealed class MenuRoleConfiguration : IEntityTypeConfiguration<MenuRole>
{
    public void Configure(EntityTypeBuilder<MenuRole> builder)
    {
        builder.ToTable("MenuRoles");

        // ⚠️ Importante: ignorar Id heredado y usar PK compuesta
        builder.Ignore(x => x.Id);
        builder.HasKey(e => new { e.MenuId, e.RoleId });

        builder.Property(x => x.State)
               .HasMaxLength(1)
               .IsUnicode(false);

        builder.HasOne(x => x.Menu)
               .WithMany()
               .HasForeignKey(x => x.MenuId);

        builder.HasOne(x => x.Role)
               .WithMany()
               .HasForeignKey(x => x.RoleId);
    }
}
