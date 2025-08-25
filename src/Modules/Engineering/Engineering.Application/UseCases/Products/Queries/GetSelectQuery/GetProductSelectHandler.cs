using Engineering.Application.Interfaces.Persistence;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Dtos.Commons;

namespace Engineering.Application.UseCases.Products.Queries.GetSelectQuery;

public class GetProductSelectHandler(IProductRepository repo)
    : IQueryHandler<GetProductSelectQuery, IEnumerable<SelectResponseDto>>
{
    private readonly IProductRepository _repo = repo;

    public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(
        GetProductSelectQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<SelectResponseDto>>();
        try
        {
            var products = await _repo.GetProductSelectAsync(request.SearchTerm);

            response.IsSuccess = true;
            response.Data = products.Adapt<IEnumerable<SelectResponseDto>>();
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
