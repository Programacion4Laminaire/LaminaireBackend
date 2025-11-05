using Identity.Application.Interfaces.RealTime;
using Microsoft.AspNetCore.SignalR;

namespace Identity.Infrastructure.RealTime;

public sealed class PermissionsNotifier(IHubContext<PermissionsHub> hub) : IPermissionsNotifier
{
    private readonly IHubContext<PermissionsHub> _hub = hub;

    public Task NotifyUserPermissionsChangedAsync(int userId, CancellationToken ct = default)
        => _hub.Clients.Group($"user:{userId}")
            .SendAsync("PermissionsChanged", cancellationToken: ct);

    public Task NotifyUsersPermissionsChangedAsync(IEnumerable<int> userIds, CancellationToken ct = default)
    {
        var tasks = userIds
            .Distinct()
            .Select(id => _hub.Clients.Group($"user:{id}")
                .SendAsync("PermissionsChanged", cancellationToken: ct));
        return Task.WhenAll(tasks);
    }
}
