
namespace Identity.Application.Dtos.RolePermissions;

public sealed record PermissionByRoleResponseDto
{
    public int PermissionId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Slug { get; init; } = null!;
    public int MenuId { get; init; }
    public string MenuName { get; init; } = null!;
    public bool Selected { get; init; }
}
