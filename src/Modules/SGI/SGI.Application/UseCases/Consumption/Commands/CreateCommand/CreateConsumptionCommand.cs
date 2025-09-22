using SharedKernel.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGI.Application.UseCases.Consumption.Commands.CreateCommand;

public record CreateConsumptionCommand : ICommand<bool>
{
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
