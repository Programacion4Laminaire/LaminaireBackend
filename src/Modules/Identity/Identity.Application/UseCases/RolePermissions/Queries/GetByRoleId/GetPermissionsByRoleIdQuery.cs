
using Identity.Application.Dtos.RolePermissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.RolePermissions.Queries.GetByRoleId;

public sealed class GetPermissionsByRoleIdQuery : IQuery<IEnumerable<PermissionByRoleResponseDto>>
{
    public int RoleId { get; init; }
}
