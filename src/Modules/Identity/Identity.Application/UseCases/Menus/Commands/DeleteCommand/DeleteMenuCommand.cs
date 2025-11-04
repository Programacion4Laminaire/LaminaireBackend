using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Menus.Commands.DeleteCommand
{
    public class DeleteMenuCommand : ICommand<bool>
    {
        public int MenuId { get; set; }
    }
}
