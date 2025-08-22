using Identity.Application.Dtos.Users;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Queries.UserRolePermissionsQuery;

public class GetUserWithRoleAndPermissionsQuery : IQuery<UserWithRoleAndPermissionsDto>
{
    public int UserId { get; set; }
}
