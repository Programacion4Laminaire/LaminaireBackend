using Identity.Application.Dtos.Roles;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Roles.Queries.GetByIdQuery;

public class GetRoleByIdQuery : IQuery<RoleByIdResponseDto>
{
    public int RoleId { get; set; }
}
