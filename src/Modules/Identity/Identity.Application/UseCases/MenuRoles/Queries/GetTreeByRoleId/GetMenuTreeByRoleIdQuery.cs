
using Identity.Application.Dtos.MenuRoles;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.MenuRoles.Queries.GetTreeByRoleId;

public sealed class GetMenuTreeByRoleIdQuery : IQuery<IEnumerable<MenuRoleTreeResponseDto>>
{
    public int RoleId { get; init; }
}
