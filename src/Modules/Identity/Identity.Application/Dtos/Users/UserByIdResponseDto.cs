namespace Identity.Application.Dtos.Users;

public record UserByIdResponseDto
{
    public int UserId { get; init; }
    public string? Identification { get; set; } = null!;
    public DateTime? BirthDate { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? State { get; init; }

    public string? ProfileImagePath { get; init; }
}
