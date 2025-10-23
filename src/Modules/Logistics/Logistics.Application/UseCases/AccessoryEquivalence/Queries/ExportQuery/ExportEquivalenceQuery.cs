using Logistics.Application.Dtos.AccessoryEquivalence;
using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.ExportQuery;

public record ExportEquivalenceQuery : IQuery<Stream>
{
    public string Sort { get; init; } = "Id";
    public string Order { get; init; } = "desc";
    public int NumFilter { get; init; } = 0;
    public string? TextFilter { get; init; }
}