using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.CreateCommand;

public class CreateCommand : ICommand<bool>
{
    public string CodigoPT { get; init; } = default!;
    public string CodigoMP { get; init; } = default!;
    public decimal Costo { get; init; }
}
