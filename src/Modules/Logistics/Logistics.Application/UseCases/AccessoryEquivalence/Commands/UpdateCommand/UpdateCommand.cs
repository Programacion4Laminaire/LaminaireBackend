using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.UpdateCommand
{
    public class UpdateCommand : ICommand<bool>
    {
        public int Id { get; init; }
        public string CodigoPT { get; init; } = default!;
        public string CodigoMP { get; init; } = default!;
        public decimal Costo { get; init; }
    }
}
