
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence
{
    public interface IUserPermissionRepository
    {
        // Overrides del usuario
        Task<List<UserPermission>> GetOverridesByUserIdAsync(int userId);
        Task<bool> ReplaceOverridesAsync(int userId, IEnumerable<UserPermission> overrides);

        // Helpers de permisos para “efectivos”
        Task<IEnumerable<int>> GetRolePermissionIdsByUserIdAsync(int userId); // por rol del usuario
        Task<List<(int PermissionId, string Name, string? Description, string Slug, int MenuId, string MenuName)>> GetAllPermissionsWithMenuAsync();
    }
}
