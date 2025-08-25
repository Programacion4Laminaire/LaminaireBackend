using Engineering.Application.Dtos.Products;
using SharedKernel.Abstractions.Messaging;

namespace Engineering.Application.UseCases.Products.Queries.GetByCodeQuery;

public class GetProductByCodeQuery : IQuery<ProductPriceResponseDto>
{
    public string Code { get; set; } = string.Empty;
}
