using Production.Application.Dtos.ReprogramLines.Responses;
using SharedKernel.Abstractions.Messaging;

namespace Production.Application.UseCases.ReprogramLines.Queries.GetOrderProductsQuery;

public class GetOrderProductsByOrderQuery : IQuery<IEnumerable<OrderProductsResponseDto>>
{
    public string? OrderNumber { get; init; }
}
