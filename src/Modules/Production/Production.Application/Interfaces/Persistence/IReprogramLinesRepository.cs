using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.UseCases.ReprogramLines.Commands;
using Production.Application.UseCases.ReprogramLines.Queries;
using Production.Application.UseCases.ReprogramLines.Queries.GetOrderProductsQuery;
using Production.Application.UseCases.ReprogramLines.Queries.ValidateOrderExistenceQuery;


namespace Production.Application.Interfaces.Persistence;

public interface IReprogramLinesRepository
{
    Task<IEnumerable<ProgrammedLinesResponseDto>> GetProgrammedLinesByOrderAndProductAsync(GetByOrderAndProductQuery programmedLines);

    Task<IEnumerable<ProgrammedLinesResponseDto>> UpdateProgrammedLinesByOrderAndProductAsync(UpdateProgrammedLinesCommand programmedLines);

    Task<IEnumerable<OrderProductsResponseDto>> GetOrderProductsByOrderAsync(GetOrderProductsByOrderQuery programmedLines);

    Task<string> ValidateOrderExistenceAsync(ValidateOrderExistenceQuery OrderNumber);

}
