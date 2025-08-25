namespace Identity.Application.Dtos.UserRole;

public record UserRoleResponseDto
{
    public int UserRoleId { get; init; }
    public string UserName { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Role { get; init; } = null!;
    public string? State { get; init; }
    public string? StateDescription { get; init; }
    public DateTime AuditCreateDate { get; init; }
}
