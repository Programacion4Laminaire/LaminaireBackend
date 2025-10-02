using SharedKernel.Abstractions.Messaging;

namespace SGI.Application.UseCases.Consumption.Commands.CreateCommand;

public class CreateConsumptionCommand : ICommand<bool>
{
    public string ResourceType { get; set; } = null!;  
    public decimal Value { get; set; }
    public string Unit { get; set; } = null!;
    public DateTime ReadingDate { get; set; }   
    public string? Note { get; set; }

}
