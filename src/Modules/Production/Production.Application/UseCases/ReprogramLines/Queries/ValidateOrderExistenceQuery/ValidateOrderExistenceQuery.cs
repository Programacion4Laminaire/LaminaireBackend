using SharedKernel.Abstractions.Messaging;

namespace Production.Application.UseCases.ReprogramLines.Queries.ValidateOrderExistenceQuery;

public class ValidateOrderExistenceQuery : IQuery<string>   
{
    public string? OrderNumber { get; init; }

}
