
namespace SGI.Domain.Entities;

public class Consumption : BaseEntity
{
    public string ResourceType { get; set; } = null!; // "ENERGY" | "GAS" | "WATER"
    public int Year { get; set; }                     // YYYY
    public int Month { get; set; }                    // 1-12
    public decimal Value { get; set; }                // Consumo
    public string Unit { get; set; } = null!;         // kWh | m³ | L (según recurso)
    public DateTime? ReadingDate { get; set; }        // fecha de lectura/cierre
    public string? MeterCode { get; set; }            // identificador medidor
    public string? Note { get; set; }                 // observaciones
}
