using Production.Application.Dtos.ReprogramLines.Requests;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.UseCases.ReprogramLines.Queries;

namespace Production.Application.Interfaces.Persistence;

public interface IReprogramLinesRepository
{
    Task<IEnumerable<ProgrammedLinesResponseDto>> GetProgrammedLinesByOrderAndProductAsync(GetByOrderAndProductQuery programmedLines);


}
