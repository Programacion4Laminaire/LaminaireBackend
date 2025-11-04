
using Identity.Application.Dtos.RolePermissions;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.RolePermissions.Commands.Upsert;

public sealed class UpsertRolePermissionsCommand : ICommand<bool>
{
    public PermissionUpsertRequestDto Request { get; init; } = null!;
}
