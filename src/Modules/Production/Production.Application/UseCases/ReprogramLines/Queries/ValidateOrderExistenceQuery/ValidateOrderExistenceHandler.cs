using Production.Application.Interfaces.Persistence;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Production.Application.UseCases.ReprogramLines.Queries.ValidateOrderExistenceQuery;

public class ValidateOrderExistenceHandler(IReprogramLinesRepository repo) : IQueryHandler<ValidateOrderExistenceQuery, string>
{
    public async Task<BaseResponse<string>> Handle(ValidateOrderExistenceQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<string>();
        try
        {
            var items = await repo.ValidateOrderExistenceAsync(request);
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




