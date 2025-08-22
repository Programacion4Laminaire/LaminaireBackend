using Identity.Application.Dtos.UserRole;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserRoles.Queries.GetByIdQuery;

public class GetUserRoleByIdQuery : IQuery<UserRoleByIdResponseDto>
{
    public int UserRoleId { get; init; }
}
