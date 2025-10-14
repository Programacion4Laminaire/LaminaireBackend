namespace Logistics.Application.Dtos.AccessoryEquivalence
{
    public record AccessoryEquivalenceCreateRequestDto
    {
        public string CodigoPT { get; init; } = default!;
        public string CodigoMP { get; init; } = default!;
        public decimal Costo { get; init; }
    }
}
