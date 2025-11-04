using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Permissions.Commands.DeleteCommand;

public class DeletePermissionCommand : ICommand<bool>
{
    public int PermissionId { get; set; }
}
