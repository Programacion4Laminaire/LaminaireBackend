namespace Identity.Application.Interfaces.RealTime;

public interface IPermissionsNotifier
{
    Task NotifyUserPermissionsChangedAsync(int userId, CancellationToken ct = default);
    Task NotifyUsersPermissionsChangedAsync(IEnumerable<int> userIds, CancellationToken ct = default);
}
