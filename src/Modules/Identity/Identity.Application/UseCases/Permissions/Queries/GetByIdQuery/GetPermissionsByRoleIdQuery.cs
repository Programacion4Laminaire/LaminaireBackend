using Identity.Application.Dtos.Permissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;

public class GetPermissionsByRoleIdQuery : IQuery<IEnumerable<PermissionsByRoleResponseDto>>
{
    public int? RoleId { get; set; }
}
