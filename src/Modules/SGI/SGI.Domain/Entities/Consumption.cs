
namespace SGI.Domain.Entities;

public class Consumption : BaseEntity
{
    public string ResourceType { get; set; } = null!;
    public decimal Value { get; set; }
    public string Unit { get; set; } = null!;
    public DateTime ReadingDate { get; set; } // requerido
    public string? Note { get; set; }
}
