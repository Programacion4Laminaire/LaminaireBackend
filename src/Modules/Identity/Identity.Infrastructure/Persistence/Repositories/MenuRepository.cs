using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Services;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context, ICurrentUserService currentUser)
            : base(context, currentUser)
        {
            _context = context;
        }

        public async Task<bool> DeleteMenuRole(List<MenuRole> menuRoles)
        {
            _context.MenuRoles.RemoveRange(menuRoles);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<IEnumerable<Menu>> GetMenuByUserIdAsync(int userId)
        {
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);

            var menus = await _context.Menus
                .AsNoTracking()
                .AsSplitQuery()
                .Join(_context.MenuRoles, m => m.Id, mr => mr.MenuId, (m, mr) => new { Menu = m, MenuRole = mr })
                .Where(x => x.MenuRole.RoleId == userRole!.RoleId && x.Menu.State == "1")
                .Select(x => x.Menu)
                .OrderBy(x => x.Position)
                .ToListAsync();

            return menus;
        }

        public async Task<IEnumerable<Menu>> GetMenuPermissionByRoleIdAsync(int? roleId)
        {
            var query = _context.Menus
                .AsNoTracking()
                .AsSplitQuery()
                .Where(m => m.Url != null && m.State == "1");

            var menus = await query.ToListAsync();
            return menus;
        }

        public async Task<List<MenuRole>> GetMenuRolesByRoleId(int roleId)
        {
            return await _context.MenuRoles
                .AsNoTracking()
                .Where(pr => pr.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<bool> RegisterRoleMenus(IEnumerable<MenuRole> menuRoles)
        {
            foreach (var menuRole in menuRoles)
            {
                menuRole.AuditCreateUser = 1;
                menuRole.AuditCreateDate = DateTime.Now;
                menuRole.State = "1";

                _context.MenuRoles.Add(menuRole);
            }

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
    }
}
