using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Production.Application.UseCases.ReprogramLines.Commands;

public class UpdateProgrammedLinesHandler(IReprogramLinesRepository repo) : IQueryHandler<UpdateProgrammedLinesCommand, IEnumerable<ProgrammedLinesResponseDto>>
{
    public async Task<BaseResponse<IEnumerable<ProgrammedLinesResponseDto>>> Handle(UpdateProgrammedLinesCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<ProgrammedLinesResponseDto>>();
        try
        {
            var items = await repo.UpdateProgrammedLinesByOrderAndProductAsync(request);
            response.IsSuccess = true;
            response.Data = items;
            response.Message = "Lineas programadas exitosamente";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}


