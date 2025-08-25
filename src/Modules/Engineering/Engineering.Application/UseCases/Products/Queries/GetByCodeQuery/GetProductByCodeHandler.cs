using Engineering.Application.Dtos.Products;
using Engineering.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Engineering.Application.UseCases.Products.Queries.GetByCodeQuery;

public class GetProductByCodeHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetProductByCodeQuery, ProductPriceResponseDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<ProductPriceResponseDto>> Handle(GetProductByCodeQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ProductPriceResponseDto>();

        try
        {
            var product = await _unitOfWork.Product.GetProductPriceByCodeAsync(request.Code);

            if (product is null)
            {
                response.IsSuccess = false;
                response.Message = "No records found.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = product.Adapt<ProductPriceResponseDto>();
            response.Message = "Query executed successfully";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
