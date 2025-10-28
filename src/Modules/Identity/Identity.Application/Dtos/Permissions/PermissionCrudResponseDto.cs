namespace Identity.Application.Dtos.Permissions;

public record PermissionCrudResponseDto
{
    public int PermissionId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Slug { get; init; } = null!;
    public int MenuId { get; init; }
    public string? State { get; init; }
    public string? StateDescription { get; init; }
    public DateTime AuditCreateDate { get; init; }
}
