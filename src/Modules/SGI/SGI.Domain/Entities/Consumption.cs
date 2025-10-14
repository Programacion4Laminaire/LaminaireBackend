
namespace SGI.Domain.Entities;

public class Consumption : BaseEntity
{
    public string ResourceType { get; set; } = null!;
    public decimal Value { get; set; }
    public string Unit { get; set; } = null!;
    public DateTime ReadingDate { get; set; }
    public string? Note { get; set; }
    public string Sede { get; set; } = null!;
    public decimal? DailyConsumption { get; set; }
}
