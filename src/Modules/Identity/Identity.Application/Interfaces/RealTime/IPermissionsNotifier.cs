namespace Identity.Application.Interfaces.RealTime;

public interface IPermissionsNotifier
{
    /// Notifica a un usuario que sus permisos efectivos cambiaron.
    Task NotifyUserPermissionsChangedAsync(int userId, CancellationToken ct = default);

    /// Notifica a varios usuarios (útil cuando cambian permisos de un rol).
    Task NotifyUsersPermissionsChangedAsync(IEnumerable<int> userIds, CancellationToken ct = default);
}
