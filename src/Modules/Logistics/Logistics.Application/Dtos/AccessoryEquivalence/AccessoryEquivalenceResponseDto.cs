namespace Logistics.Application.Dtos.AccessoryEquivalence;

public record AccessoryEquivalenceResponseDto
{
    public int Id { get; init; }
    public string CodigoPT { get; init; } = default!;
    public string DescripcionPT { get; init; } = default!;
    public string CodigoMP { get; init; } = default!;
    public string DescripcionMP { get; init; } = default!;
    public decimal Costo { get; init; }
    public DateTime FechaCreacion { get; init; }
}
