// Infrastructure/Persistence/Repositories/UserPermissionRepository.cs
using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public UserPermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserPermission>> GetOverridesByUserIdAsync(int userId)
        {
            return await _context.UserPermissions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ReplaceOverridesAsync(int userId, IEnumerable<UserPermission> overrides)
        {
            // Elimina overrides actuales del usuario
            var current = await _context.UserPermissions
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (current.Count > 0)
                _context.UserPermissions.RemoveRange(current);

            // Inserta los nuevos con auditoría/estado
            var now = DateTime.Now;
            foreach (var ov in overrides)
            {
                ov.AuditCreateUser = 1;           // <-- coloca el usuario actual si lo tienes en ICurrentUserService
                ov.AuditCreateDate = now;
                ov.State = ov.State ?? "1";
                _context.UserPermissions.Add(ov);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<int>> GetRolePermissionIdsByUserIdAsync(int userId)
        {
            // Asumimos un solo rol por usuario (como ya usas en Menú); ajusta si hay muchos.
            var userRole = await _context.UserRoles.AsNoTracking()
                                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userRole is null) return Enumerable.Empty<int>();

            var ids = await _context.RolePermissions
                .AsNoTracking()
                .Where(rp => rp.RoleId == userRole.RoleId && rp.State == "1")
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            return ids;
        }

        public async Task<List<(int PermissionId, string Name, string? Description, string Slug, int MenuId, string MenuName)>> GetAllPermissionsWithMenuAsync()
        {
            return await _context.Permissions
                .AsNoTracking()
                .Where(p => p.AuditDeleteUser == null && p.AuditDeleteDate == null && p.State == "1")
                .Join(_context.Menus, p => p.MenuId, m => m.Id, (p, m) => new { p, m })
                .OrderBy(x => x.m.Position).ThenBy(x => x.p.Name)
                .Select(x => new ValueTuple<int, string, string?, string, int, string>(
                    x.p.Id, x.p.Name, x.p.Description, x.p.Slug, x.p.MenuId, x.m.Name))
                .ToListAsync();
        }
    }
}
