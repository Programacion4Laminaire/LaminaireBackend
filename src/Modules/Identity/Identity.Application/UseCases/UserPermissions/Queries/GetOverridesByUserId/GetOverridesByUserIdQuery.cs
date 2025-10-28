
using Identity.Application.Dtos.UserPermissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserPermissions.Queries.GetOverridesByUserId;

public sealed class GetOverridesByUserIdQuery : IQuery<IEnumerable<UserPermissionOverrideDto>>
{
    public int UserId { get; init; }
}
