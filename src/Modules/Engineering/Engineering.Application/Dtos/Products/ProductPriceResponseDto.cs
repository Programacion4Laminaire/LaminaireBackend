

namespace Engineering.Application.Dtos.Products;

public class ProductPriceResponseDto
{
    public string? Code { get; set; }
    public string? ProductName { get; set; }
    public string? SublineCode { get; set; }
    public string?   SublineName { get; set; }
    public decimal Cost { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal Margin { get; set; }
    public decimal SalePriceCop { get; set; }
    public decimal SalePriceUsd { get; set; }
    public decimal Multiplier { get; set; }
    public decimal DistributorMultiplier { get; set; }
    public decimal BasePriceUsd { get; set; }
}
