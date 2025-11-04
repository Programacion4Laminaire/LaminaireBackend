// Identity.Application/Interfaces/Persistence/IMenuRepository.cs
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetMenuByUserIdAsync(int userId);
        Task<IEnumerable<Menu>> GetMenuPermissionByRoleIdAsync(int? roleId);

        // Menú–Rol
        Task<bool> RegisterRoleMenus(IEnumerable<MenuRole> menuRoles);
        Task<List<MenuRole>> GetMenuRolesByRoleId(int roleId);
        Task<bool> DeleteMenuRole(List<MenuRole> menuRoles);
    }
}
