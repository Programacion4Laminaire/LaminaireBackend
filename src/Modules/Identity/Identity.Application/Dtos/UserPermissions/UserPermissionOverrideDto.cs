
namespace Identity.Application.Dtos.UserPermissions;

public sealed record UserPermissionOverrideDto
{
    public int PermissionId { get; init; }
    public bool IsGranted { get; init; } // true: grant; false: deny
}
