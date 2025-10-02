using SGI.Application.Dtos.Consumption;
using SharedKernel.Abstractions.Messaging;

namespace SGI.Application.UseCases.Consumption.Queries.GetByIdQuery;

public class GetConsumptionByIdQuery : IQuery<ConsumptionByIdResponseDto>
{
    public int ConsumptionId { get; set; }
}