
namespace Identity.Application.Dtos.UserPermissions;

public sealed record UserPermissionOverrideUpsertRequestDto
{
    public int UserId { get; init; }
    public IEnumerable<UserPermissionOverrideDto> Overrides { get; init; } = Array.Empty<UserPermissionOverrideDto>();
}
