using Engineering.Application.Dtos.Products;
using SharedKernel.Dtos.Commons;

namespace Engineering.Application.Interfaces.Persistence;

public interface IProductRepository
{
    Task<ProductPriceResponseDto?> GetProductPriceByCodeAsync(string code);
   
    Task<IEnumerable<SelectResponseDto>> GetProductSelectAsync(string? searchTerm);
}
