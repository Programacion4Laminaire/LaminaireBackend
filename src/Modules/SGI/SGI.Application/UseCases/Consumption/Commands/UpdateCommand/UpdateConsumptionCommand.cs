using SharedKernel.Abstractions.Messaging;

namespace SGI.Application.UseCases.Consumption.Commands.UpdateCommand;

public record UpdateConsumptionCommand : ICommand<bool>
{
    public int ConsumptionId { get; init; }
    public string ResourceType { get; init; } = null!;
    public int Year { get; init; }
    public int Month { get; init; }
    public decimal Value { get; init; }
    public string Unit { get; init; } = null!;
    public DateTime? ReadingDate { get; init; }
    public string? MeterCode { get; init; }
    public string? Note { get; init; }
    public string State { get; init; } = "1";
}