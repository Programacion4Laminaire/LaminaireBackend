
namespace Identity.Application.Dtos.MenuRoles;

public record MenuRoleUpsertRequestDto
{
    public int RoleId { get; init; }
    public IEnumerable<int> MenuIds { get; init; } = Array.Empty<int>();
}
