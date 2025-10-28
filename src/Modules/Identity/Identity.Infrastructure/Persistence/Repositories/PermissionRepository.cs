using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Services;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context, ICurrentUserService currentUser)
            : base(context, currentUser)
        {
            _context = context;
        }

        public async Task<bool> DeleteRolePermission(List<RolePermission> permissions)
        {
            _context.RolePermissions.RemoveRange(permissions);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<List<RolePermission>> GetPermissionRolesByRoleId(int roleId)
        {
            return await _context.RolePermissions
                    .AsNoTracking()
                    .Where(pr => pr.RoleId == roleId)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByMenuId(int menuId)
        {
            var menuPermissions = await _context.Permissions
                    .AsNoTracking()
                    .Where(x => x.MenuId == menuId && x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                    .ToListAsync();

            return menuPermissions;
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsByMenuId(int roleId, int menuId)
        {
            var rolePermissions = _context.RolePermissions
                    .Where(pr => pr.RoleId == roleId && pr.Permission.MenuId == menuId)
                    .Select(pr => pr.Permission)
                    .AsNoTracking();

            var data = await rolePermissions.ToListAsync(); // materializa
            return data;
        }

        public async Task<bool> RegisterRolePermissions(IEnumerable<RolePermission> rolePermissions)
        {
            foreach (var rp in rolePermissions)
            {
                rp.AuditCreateUser = 1;
                rp.AuditCreateDate = DateTime.Now;
                rp.State = "1";
                _context.RolePermissions.Add(rp);
            }

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
    }
}
