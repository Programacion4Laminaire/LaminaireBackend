
using Identity.Application.Dtos.MenuRoles;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.MenuRoles.Commands.Upsert;

public class UpsertMenuRolesCommand : ICommand<bool>
{
    public MenuRoleUpsertRequestDto Request { get; init; } = null!;
}
