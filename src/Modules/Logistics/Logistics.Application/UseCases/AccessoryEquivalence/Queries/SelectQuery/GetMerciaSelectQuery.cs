using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.SelectQuery
{

    public class GetMerciaSelectQuery : IQuery<IEnumerable<SelectResponseDto>>
    {
        public string? SearchTerm { get; init; }
        /// <summary>PT = códigos >= 15 dígitos; MP = exactamente 5 dígitos.</summary>
        public string? Kind { get; init; } // "PT" | "MP"
    }
}
