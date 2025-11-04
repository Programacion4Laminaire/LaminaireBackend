
using Identity.Application.Dtos.MenuRoles;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.MenuRoles.Queries.GetByRoleIdQuery;

public class GetMenusByRoleIdQuery : IQuery<IEnumerable<MenuRoleByRoleResponseDto>>
{
    public int RoleId { get; init; }
}
