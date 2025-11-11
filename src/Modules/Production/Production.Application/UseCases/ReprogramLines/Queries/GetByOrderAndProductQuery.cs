using Production.Application.Dtos.ReprogramLines.Responses;
using SharedKernel.Abstractions.Messaging;

namespace Production.Application.UseCases.ReprogramLines.Queries;

public class GetByOrderAndProductQuery : IQuery<ProgrammedLinesResponseDto>
{
    public string? OrderNumber { get; init; }
    public string? ProductCode { get; init; }
}
