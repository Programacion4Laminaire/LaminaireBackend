namespace Production.Application.Dtos.ReprogramLines.Responses;

public class OrderProductsResponseDto
{
    public string? ProductCode { get; set; }
    public string? ProductDescription { get; set; }
    public decimal Quantity { get; set; }
}
