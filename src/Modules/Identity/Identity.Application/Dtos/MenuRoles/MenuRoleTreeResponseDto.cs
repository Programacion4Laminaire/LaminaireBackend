
namespace Identity.Application.Dtos.MenuRoles;

public sealed class MenuRoleTreeResponseDto
{
    public int MenuId { get; init; }
    public int? FatherId { get; init; }
    public string Name { get; init; } = null!;
    public string? Icon { get; init; }
    public string? Url { get; init; }
    public bool Selected { get; set; }
    public int Position { get; init; }

    public List<MenuRoleTreeResponseDto> Children { get; init; } = new();
}
