namespace Identity.Infrastructure.Authentication;

public interface IPermissionService
{
    /// Devuelve el conjunto de slugs efectivos del usuario (Role ± UserPermission overrides).
    Task<HashSet<string>> GetEffectivePermissionSlugsAsync(int userId, CancellationToken ct = default);
}
