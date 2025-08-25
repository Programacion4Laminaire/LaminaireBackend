namespace Engineering.Application.Dtos.Products;

public record ProductPriceResponseDto
{
    public string? Code { get; init; }
    public string? ProductName { get; init; }
    public string? SublineCode { get; init; }
    public string? SublineName { get; init; }
    public decimal Cost { get; init; }
    public decimal ExchangeRate { get; init; }
    public decimal Margin { get; init; }
    public decimal SalePriceCop { get; init; }
    public decimal SalePriceUsd { get; init; }
    public decimal Multiplier { get; init; }
    public decimal DistributorMultiplier { get; init; }
    public decimal BasePriceUsd { get; init; }
}
