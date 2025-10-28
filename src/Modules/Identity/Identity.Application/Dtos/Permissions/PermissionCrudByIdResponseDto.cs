namespace Identity.Application.Dtos.Permissions;

public record PermissionCrudByIdResponseDto
{
    public int PermissionId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Slug { get; init; } = null!;
    public int MenuId { get; init; }
    public string? State { get; init; }
}
