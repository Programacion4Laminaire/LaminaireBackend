using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Menus.Commands.CreateCommand
{
    public class CreateMenuCommand : ICommand<bool>
    {
        public int Position { get; set; }
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Url { get; set; }
        public int? FatherId { get; set; }
        public string? State { get; set; } // "1" | "0"
        public bool IsNew { get; set; }
    }
}
