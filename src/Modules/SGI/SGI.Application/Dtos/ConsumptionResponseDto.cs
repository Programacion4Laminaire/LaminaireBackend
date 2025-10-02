namespace SGI.Application.Dtos.Consumption;

public record ConsumptionResponseDto
{
    public int ConsumptionId { get; init; }
    public string ResourceType { get; init; } = null!;  
    public decimal Value { get; init; }
    public string Unit { get; init; } = null!;
    public DateTime? ReadingDate { get; init; }   
    public string? Note { get; init; }  
    public DateTime AuditCreateDate { get; init; }
}
