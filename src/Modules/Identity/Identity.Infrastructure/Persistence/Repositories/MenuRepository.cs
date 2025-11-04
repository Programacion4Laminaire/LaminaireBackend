// Identity.Infrastructure/Persistence/Repositories/MenuRepository.cs
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
        private readonly ICurrentUserService _currentUser;

        public MenuRepository(ApplicationDbContext context, ICurrentUserService currentUser)
            : base(context, currentUser)
        {
            _context = context;
            _currentUser = currentUser;
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

        // IMenuRepository / MenuRepository
        public async Task<IEnumerable<Menu>> GetMenuPermissionByRoleIdAsync(int? roleId)
        {
            var query = _context.Menus
                .AsNoTracking()
                .AsSplitQuery()             
                .Where(m => m.State == "1");

            return await query.ToListAsync();
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
            var now = DateTime.UtcNow;
            var uid = _currentUser.UserId ?? 1;

            foreach (var mr in menuRoles)
            {
                mr.AuditCreateUser = uid;
                mr.AuditCreateDate = now;
                mr.State = "1";
                _context.MenuRoles.Add(mr);
            }

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
    }
}
