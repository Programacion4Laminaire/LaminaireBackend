using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Permissions.Commands.CreateCommand;

public class CreatePermissionCommand : ICommand<bool>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Slug { get; set; } = null!;
    public int MenuId { get; set; }
    public string? State { get; set; }  // "1" | "0"
}
