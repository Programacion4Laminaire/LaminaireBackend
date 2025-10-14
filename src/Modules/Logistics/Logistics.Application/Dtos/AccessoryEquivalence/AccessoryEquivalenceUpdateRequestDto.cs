namespace Logistics.Application.Dtos.AccessoryEquivalence;

public record AccessoryEquivalenceUpdateRequestDto
{
    public int Id { get; init; }
    public string CodigoPT { get; init; } = default!;
    public string CodigoMP { get; init; } = default!;
    public decimal Costo { get; init; }
}
