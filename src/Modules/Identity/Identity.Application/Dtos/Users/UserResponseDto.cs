namespace Identity.Application.Dtos.Users;

public record UserResponseDto
{
    public int UserId { get; init; }
    public string? Identification { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? State { get; init; }
    public string? StateDescription { get; init; }
    public DateTime AuditCreateDate { get; init; }
    public string? ProfileImagePath { get; init; }
}
