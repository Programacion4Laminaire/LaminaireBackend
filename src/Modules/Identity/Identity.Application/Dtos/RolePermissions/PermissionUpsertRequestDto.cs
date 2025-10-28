
namespace Identity.Application.Dtos.RolePermissions;

public sealed record PermissionUpsertRequestDto
{
    public int RoleId { get; init; }
    public IEnumerable<int> PermissionIds { get; init; } = Array.Empty<int>();
}
