
using System.Text.Json.Serialization;

namespace Identity.Application.Dtos.MenuRoles;

public record MenuRoleByRoleResponseDto
{
    public int MenuId { get; init; }
    public int? FatherId { get; init; }
    public string Name { get; init; } = null!;
    public string? Icon { get; init; }
    public string? Url { get; init; }

    [JsonPropertyName("selected")]
    public bool Selected { get; init; }
}
