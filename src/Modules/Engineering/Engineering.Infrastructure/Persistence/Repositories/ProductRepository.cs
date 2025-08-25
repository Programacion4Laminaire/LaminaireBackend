using Dapper;
using Engineering.Application.Dtos.Products;
using Engineering.Application.Interfaces.Persistence;
using Engineering.Infrastructure.Persistence.Context;
using SharedKernel.Dtos.Commons;

namespace Engineering.Infrastructure.Persistence.Repositories;

public class ProductRepository(LaminaireDbContext context) : IProductRepository
{
    private readonly LaminaireDbContext _context = context;

    public async Task<ProductPriceResponseDto?> GetProductPriceByCodeAsync(string code)
    {
        var sql = @"
    SELECT 
    LTRIM(RTRIM(M.Codigo)) AS Code, 
    LTRIM(RTRIM(M.DESCRIPCIO)) AS ProductName, 
    LTRIM(RTRIM(M.CODSBLIN)) AS SublineCode, 
    LTRIM(RTRIM(S.NOMBRE)) AS SublineName,   
    CAST(X.Precio AS DECIMAL(18,2)) AS Cost,	
    (SELECT TOP 1 VALOR 
     FROM CILAMINAIRE..MTCAMBIO 
     WHERE FECHA = CAST(GETDATE() AS DATE) 
     ORDER BY FECHA DESC) AS ExchangeRate,
    0.7 AS Margin,
    CAST(X.Precio / (1 - 0.7) AS DECIMAL(18,2)) AS SalePriceCop,
    CAST((X.Precio / (1 - 0.7)) / 
         (SELECT TOP 1 VALOR 
          FROM CILAMINAIRE..MTCAMBIO 
          WHERE FECHA = CAST(GETDATE() AS DATE) 
          ORDER BY FECHA DESC) AS DECIMAL(18,2)) AS SalePriceUsd,
    Y.Multiplicador AS Multiplier,
    Y.Multiplicador AS DistributorMultiplier,
    CAST(((X.Precio / (1 - 0.7)) / 
          (SELECT TOP 1 VALOR 
           FROM CILAMINAIRE..MTCAMBIO 
           WHERE FECHA = CAST(GETDATE() AS DATE) 
           ORDER BY FECHA DESC)) / Y.Multiplicador AS DECIMAL(18,2)) AS BasePriceUsd
FROM CILAMINAIRE..MTMERCIA M
INNER JOIN CILAMINAIRE..MTSBLIN S ON S.CODSBLIN = M.CODSBLIN
INNER JOIN TBL_PRECIOS_BASE_EXPO X ON X.Producto = M.CODIGO
INNER JOIN TBL_MULTIPLICADORES Y ON Y.Sublinea = M.CODSBLIN
WHERE M.CODIGO = @Code;

        ";

        using var connection = _context.CreateConnection;
        return await connection.QueryFirstOrDefaultAsync<ProductPriceResponseDto>(sql, new { Code = code });
    }
    public async Task<IEnumerable<SelectResponseDto>> GetProductSelectAsync(string? searchTerm)
    {
        var sql = @"
        SELECT TOP 20
            LTRIM(RTRIM(CODIGO)) AS Code,
            LTRIM(RTRIM(DESCRIPCIO)) AS Description
        FROM CILAMINAIRE..MTMERCIA
        WHERE (@SearchTerm IS NULL 
               OR CODIGO LIKE '%' + @SearchTerm + '%' 
               OR DESCRIPCIO LIKE '%' + @SearchTerm + '%')
        ORDER BY DESCRIPCIO;";

        using var connection = _context.CreateConnection;
        return await connection.QueryAsync<SelectResponseDto>(sql, new { SearchTerm = searchTerm });
    }


}
