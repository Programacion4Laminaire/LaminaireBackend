using SharedKernel.Abstractions.Messaging;

namespace Engineering.Application.UseCases.Products.Commands.UpdateCommand;

public class UpdateProductCommand : ICommand<bool>
{
    public string Code { get; set; } = default!;
    public string SublineCode { get; set; } = default!;
    public decimal Cost { get; set; }
    public decimal Multiplier { get; set; }
    public decimal DistributorMultiplier { get; set; }
    public decimal Margin { get; init; }           // fracción (0.55–0.70)
    public decimal BasePriceUsd { get; set; }
}
