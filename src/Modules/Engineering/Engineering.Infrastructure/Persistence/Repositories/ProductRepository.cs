using Dapper;
using Engineering.Application.Dtos.Products;
using Engineering.Application.Interfaces.Persistence;
using Engineering.Infrastructure.Persistence.Context;
using SharedKernel.Dtos.Commons;

namespace Engineering.Infrastructure.Persistence.Repositories;

public class ProductRepository(EngineeringDbContext context) : IProductRepository
{
    private readonly EngineeringDbContext _context = context;

    public async Task<ProductPriceResponseDto?> GetProductPriceByCodeAsync(string code)
    {
        var sql = @"
SELECT 
    LTRIM(RTRIM(M.Codigo))        AS Code, 
    LTRIM(RTRIM(M.DESCRIPCIO))    AS ProductName, 
    LTRIM(RTRIM(M.CODSBLIN))      AS SublineCode, 
    LTRIM(RTRIM(S.NOMBRE))        AS SublineName,   
    CAST(X.Costo AS DECIMAL(18,2))                         AS Cost,	

    -- Tasa de cambio (última disponible <= hoy)
    LR.VALOR                                            AS ExchangeRate,

    -- Margen editable (si no hay, usa 0.70 por defecto)
    ISNULL(X.Margen, 0.70)                              AS Margin,

    -- Precio Venta COP = Costo / (1 - Margen)
    CAST(X.Costo / (1 - ISNULL(X.Margen,0.70)) AS DECIMAL(18,2)) AS SalePriceCop,

    -- Precio Venta USD = Precio Venta COP / ExchangeRate
    CAST((X.Costo / (1 - ISNULL(X.Margen,0.70))) / NULLIF(LR.VALOR,0) 
         AS DECIMAL(18,2))                               AS SalePriceUsd,

    -- Multiplicadores
    Y.Multiplicador                                     AS Multiplier,
    Y.MultiplicaDistribuidor                            AS DistributorMultiplier,

    -- BasePriceUsd:
    -- Si existe X.Precio (>0), úsalo. Si no, calcula:
    -- ((Costo / (1 - Margen)) / TC) / Multiplicador
    CAST(
        CASE 
            WHEN X.Precio IS NOT NULL AND X.Precio > 0 
                THEN X.Precio
            ELSE ((X.Costo / (1 - ISNULL(X.Margen,0.70))) / NULLIF(LR.VALOR,0)) / NULLIF(Y.Multiplicador,0)
        END
        AS DECIMAL(18,2)
    ) AS BasePriceUsd
FROM CILAMINAIRE..MTMERCIA M
LEFT JOIN CILAMINAIRE..MTSBLIN S 
       ON S.CODSBLIN = M.CODSBLIN
LEFT JOIN TBL_PRECIOS_BASE_EXPO X 
       ON X.Producto = M.CODIGO
LEFT JOIN TBL_MULTIPLICADORES Y 
       ON Y.Sublinea = M.CODSBLIN
OUTER APPLY (
    SELECT TOP 1 VALOR
    FROM CILAMINAIRE..MTCAMBIO 
    WHERE FECHA <= CAST(GETDATE() AS DATE)
    ORDER BY FECHA DESC
) LR
WHERE M.CODIGO = LTRIM(RTRIM(@Code));
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
          AND LEN(RTRIM(CODIGO))>15
        ORDER BY DESCRIPCIO;";

        using var connection = _context.CreateConnection;
        return await connection.QueryAsync<SelectResponseDto>(sql, new { SearchTerm = searchTerm });
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateRequestDto dto)
    {
        var sql = @"
SET XACT_ABORT ON;
BEGIN TRAN;

-- 1) Actualizar Sublinea del producto
UPDATE CILAMINAIRE..MTMERCIA
SET CODSBLIN = @SublineCode
WHERE CODIGO = @Code;

-- 2) Preparar TC
DECLARE @TC DECIMAL(18,6) =
(
    SELECT TOP 1 VALOR
    FROM CILAMINAIRE..MTCAMBIO
    WHERE FECHA <= CAST(GETDATE() AS DATE)
    ORDER BY FECHA DESC
);

-- 3) Calcular o usar BaseUSD
DECLARE @BaseUSD DECIMAL(18,2) =
CASE 
    WHEN @BasePriceUsd IS NOT NULL AND @BasePriceUsd > 0 
        THEN @BasePriceUsd   -- si vino del frontend, se respeta
    WHEN ISNULL(@TC,0) = 0 OR ISNULL(@Multiplier,0) = 0 
        THEN NULL
    ELSE CAST(ROUND(((@Cost / (1 - @Margin)) / @TC) / @Multiplier, 2) AS DECIMAL(18,2))
END;

-- 4) Actualizar costo, multiplicadores, margen y precio
UPDATE TBL_PRECIOS_BASE_EXPO
SET Costo = @Cost,
    Multiplicador = @Multiplier,
    MultiplicaDistribuidor = @DistributorMultiplier,
    Margen = @Margin,
    Precio = COALESCE(@BaseUSD, Precio)
WHERE Producto = @Code;

-- 5) Actualizar multiplicadores a nivel de Sublinea
UPDATE TBL_MULTIPLICADORES
SET Multiplicador = @Multiplier,
    MultiplicaDistribuidor = @DistributorMultiplier
WHERE Sublinea = @SublineCode;

-- 6) Actualizar listas de precio
UPDATE MP SET PRECIO = P.Precio * P.MultiplicaDistribuidor
FROM TBL_PRECIOS_BASE_EXPO P 
INNER JOIN CILAMINAIRE..MTMERCIA M ON M.CODIGO = P.Producto 
INNER JOIN CILAMINAIRE..MVPRECIO MP ON MP.CODPRODUC = P.Producto AND MP.CODPRECIO = 'DIS-1' 
WHERE P.Producto = @Code
  AND P.MultiplicaDistribuidor IS NOT NULL 
  AND CAST(P.Precio * P.MultiplicaDistribuidor AS numeric(28,15)) <> MP.PRECIO;

UPDATE MP SET PRECIO = P.Precio * P.Multiplicador
FROM TBL_PRECIOS_BASE_EXPO P 
INNER JOIN CILAMINAIRE..MTMERCIA M ON M.CODIGO = P.Producto 
INNER JOIN CILAMINAIRE..MVPRECIO MP ON MP.CODPRODUC = P.Producto AND MP.CODPRECIO = 'USA-1' 
WHERE P.Producto = @Code
  AND P.Multiplicador IS NOT NULL 
  AND CAST(P.Precio * P.Multiplicador AS numeric(15,7)) <> MP.PRECIO;

COMMIT;
";

        using var connection = _context.CreateConnection;
        var rows = await connection.ExecuteAsync(sql, dto);
        return rows > 0;
    }

}
