using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateUserPasswordByIdentityCommand : ICommand<bool>
{
    public string Identification { get; set; } = null!;
    public string UserName { get; init; } = null!;
    public DateTime BirthDate { get; set; }
    public string NewPassword { get; set; } = null!;
}