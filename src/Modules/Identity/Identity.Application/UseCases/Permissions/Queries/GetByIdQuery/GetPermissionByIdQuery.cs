using Identity.Application.Dtos.Permissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;

public class GetPermissionByIdQuery : IQuery<PermissionCrudByIdResponseDto>
{
    public int PermissionId { get; set; }
}
