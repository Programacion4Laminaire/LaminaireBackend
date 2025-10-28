
using Identity.Application.Dtos.UserPermissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserPermissions.Queries.GetEffectiveByUserId;

public sealed class GetEffectivePermissionsByUserIdQuery : IQuery<IEnumerable<UserPermissionByUserResponseDto>>
{
    public int UserId { get; init; }
}
