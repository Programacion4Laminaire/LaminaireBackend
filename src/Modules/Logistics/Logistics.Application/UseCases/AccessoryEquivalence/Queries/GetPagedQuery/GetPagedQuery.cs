using Logistics.Application.Dtos.AccessoryEquivalence;
using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetPagedQuery;

public class GetPagedQuery : IQuery<IEnumerable<AccessoryEquivalenceResponseDto>>
{
    public int Records { get; init; } = 10;
    public int NumPage { get; init; } = 1;
    public string Sort { get; init; } = "Id";
    public string Order { get; init; } = "desc";
    public int NumFilter { get; init; } = 0;
    public string? TextFilter { get; init; }
}
