using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<bool> RegisterRolePermissions(IEnumerable<RolePermission> rolePermissions);
        Task<IEnumerable<Permission>> GetPermissionsByMenuId(int menuId);
        Task<IEnumerable<Permission>> GetRolePermissionsByMenuId(int roleId, int menuId);
        Task<List<RolePermission>> GetPermissionRolesByRoleId(int roleId);
        Task<bool> DeleteRolePermission(List<RolePermission> permissions);

        // —— EXTENSIONES PARA LISTAR/UPSERT POR ROL (NUEVO) ——
        /// <summary>Proyección liviana de todos los permisos activos con el nombre del menú.</summary>
        Task<List<(int PermissionId, string Name, string? Description, string Slug, int MenuId, string MenuName)>> GetAllWithMenuAsync();

        /// <summary>Devuelve todos los RolePermission por RoleId (para limpiar/upsert).</summary>
        Task<List<RolePermission>> GetRolePermissionsByRoleIdAsync(int roleId);

        /// <summary>Devuelve sólo los IDs de permiso asignados a un rol.</summary>
        Task<IEnumerable<int>> GetPermissionIdsByRoleIdAsync(int roleId);

        /// <summary>Borra en bloque RolePermissions (para upsert por rol).</summary>
        Task<bool> DeleteRolePermissionsAsync(List<RolePermission> rolePermissions);

        /// <summary>Registra en bloque RolePermissions (para upsert por rol).</summary>
        Task<bool> RegisterRolePermissionsAsync(IEnumerable<RolePermission> rolePermissions);
    }
}
