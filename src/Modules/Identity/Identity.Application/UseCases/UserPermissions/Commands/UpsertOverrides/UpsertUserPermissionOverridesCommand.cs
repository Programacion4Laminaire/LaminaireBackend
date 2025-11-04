
using Identity.Application.Dtos.UserPermissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.UserPermissions.Commands.UpsertOverrides;

public sealed class UpsertUserPermissionOverridesCommand : ICommand<bool>
{
    public UserPermissionOverrideUpsertRequestDto Request { get; init; } = null!;
}
