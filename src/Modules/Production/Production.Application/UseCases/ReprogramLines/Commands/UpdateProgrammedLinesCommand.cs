using Production.Application.Dtos.ReprogramLines.Responses;
using SharedKernel.Abstractions.Messaging;

namespace Production.Application.UseCases.ReprogramLines.Commands
{
    public class UpdateProgrammedLinesCommand : IQuery<IEnumerable<ProgrammedLinesResponseDto>>
    {
        public string? OrderNumber { get; init; }
        public string? ProductCode { get; init; }
        public string? BatchNumber { get; init; }
        public string? UserCode { get; init; }
    }
}
