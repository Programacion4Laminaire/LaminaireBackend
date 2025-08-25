using Engineering.Application.Dtos.Products;

namespace Engineering.Application.Interfaces.Persistence;

public interface IProductRepository
{
    Task<ProductPriceResponseDto?> GetProductPriceByCodeAsync(string code);
}
