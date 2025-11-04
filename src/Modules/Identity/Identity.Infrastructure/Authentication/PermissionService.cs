using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Authentication;

public sealed class PermissionService(ApplicationDbContext db) : IPermissionService
{
    private readonly ApplicationDbContext _db = db;

    public async Task<HashSet<string>> GetEffectivePermissionSlugsAsync(int userId, CancellationToken ct = default)
    {
        // 1) permisos por rol
        var roleId = await _db.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => (int?)ur.RoleId)
            .FirstOrDefaultAsync(ct);

        var roleSlugs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (roleId is not null)
        {
            var rslugs = await _db.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleId == roleId && rp.State == "1")
                .Join(_db.Permissions,
                      rp => rp.PermissionId,
                      p => p.Id,
                      (rp, p) => p.Slug)
                .ToListAsync(ct);
            roleSlugs = new HashSet<string>(rslugs, StringComparer.OrdinalIgnoreCase);
        }

        // 2) overrides de usuario
        var overrides = await _db.UserPermissions
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .Join(_db.Permissions, up => up.PermissionId, p => p.Id, (up, p) => new { up.IsGranted, p.Slug })
            .ToListAsync(ct);

        // 3) aplicar overrides sobre el baseline del rol
        foreach (var o in overrides)
        {
            if (o.IsGranted) roleSlugs.Add(o.Slug);
            else roleSlugs.Remove(o.Slug);
        }

        return roleSlugs;
    }
}
