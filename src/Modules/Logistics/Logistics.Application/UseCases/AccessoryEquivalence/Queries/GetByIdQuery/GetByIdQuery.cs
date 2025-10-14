using Logistics.Application.Dtos.AccessoryEquivalence;
using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries;

public class GetByIdQuery : IQuery<AccessoryEquivalenceResponseDto>
{
    public int Id { get; init; }
}
