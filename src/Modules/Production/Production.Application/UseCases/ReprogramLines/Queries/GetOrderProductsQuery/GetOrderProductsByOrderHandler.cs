using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Production.Application.UseCases.ReprogramLines.Queries.GetOrderProductsQuery;

public class GetOrderProductsByOrderHandler(IReprogramLinesRepository repo) : IQueryHandler<GetOrderProductsByOrderQuery, IEnumerable<OrderProductsResponseDto>>
{
    public async Task<BaseResponse<IEnumerable<OrderProductsResponseDto>>> Handle(GetOrderProductsByOrderQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<OrderProductsResponseDto>>();
        try
        {
            var items = await repo.GetOrderProductsByOrderAsync(request);
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



