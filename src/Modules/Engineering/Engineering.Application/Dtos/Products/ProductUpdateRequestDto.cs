namespace Engineering.Application.Dtos.Products;

public record ProductUpdateRequestDto
{
    public string Code { get; init; } = default!;
    public string SublineCode { get; init; } = default!;
    public decimal Cost { get; init; }
    public decimal Multiplier { get; init; }
    public decimal DistributorMultiplier { get; init; }
    public decimal BasePriceUsd { get; init; }
    public decimal Margin { get; init; }   // nuevo
}
