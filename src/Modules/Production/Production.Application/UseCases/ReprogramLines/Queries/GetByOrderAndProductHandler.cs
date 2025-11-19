using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Production.Application.UseCases.ReprogramLines.Queries;

public class GetByOrderAndProductHandler(IReprogramLinesRepository repo)
     : IQueryHandler<GetByOrderAndProductQuery, IEnumerable<ProgrammedLinesResponseDto>>
{
    public async Task<BaseResponse<IEnumerable<ProgrammedLinesResponseDto>>> Handle(GetByOrderAndProductQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<ProgrammedLinesResponseDto>>();
        try
        {
            var items = await repo.GetProgrammedLinesByOrderAndProductAsync(request);
            response.IsSuccess = true;
            response.Data = items;
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}



