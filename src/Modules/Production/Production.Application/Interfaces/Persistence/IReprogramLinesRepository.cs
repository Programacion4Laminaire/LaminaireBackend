using Production.Application.Dtos.ReprogramLines.Requests;
using Production.Application.Dtos.ReprogramLines.Responses;

namespace Production.Application.Interfaces.Persistence;

public interface IReprogramLinesRepository
{
    Task<IEnumerable<ProgrammedLinesResponseDto>> GetMerciaSelectAsync(ProgrammedLinesRequestDto programmedLines);
}
