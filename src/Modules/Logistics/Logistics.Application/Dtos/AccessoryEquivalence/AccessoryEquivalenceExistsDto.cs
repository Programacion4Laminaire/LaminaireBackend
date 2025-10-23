namespace Logistics.Application.Dtos.AccessoryEquivalence
{
    public record AccessoryEquivalenceExistsDto
    {
        public string CodigoMP { get; init; } = default!;
        public string CodigoPT { get; init; } = default!;
        public int? ExcludeId { get; init; }  
    }
}
