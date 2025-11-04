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
        // SELECT robusto: si Costo es NULL o 0, se reconstruye desde Precio Base USD * Multiplicador * TC * (1 - Margen)
        var sql = @"
WITH TC AS (
    SELECT TOP 1 VALOR
    FROM CILAMINAIRE..MTCAMBIO 
    WHERE FECHA <= CAST(GETDATE() AS DATE)
    ORDER BY FECHA DESC
)
SELECT 
    LTRIM(RTRIM(M.CODIGO))     AS Code, 
    LTRIM(RTRIM(M.DESCRIPCIO)) AS ProductName, 
    LTRIM(RTRIM(M.CODSBLIN))   AS SublineCode, 
    LTRIM(RTRIM(S.NOMBRE))     AS SublineName,   

    (SELECT VALOR FROM TC)                     AS ExchangeRate,
    ISNULL(X.Margen, 0.70)                     AS Margin,

    -- Costo efectivo: valor guardado si > 0; si no, reconstruido
    CAST(
      CASE 
        WHEN X.Costo IS NOT NULL AND X.Costo > 0 
          THEN X.Costo
        WHEN X.Precio IS NOT NULL AND X.Precio > 0 
             AND Y.Multiplicador IS NOT NULL AND Y.Multiplicador > 0
             AND (SELECT VALOR FROM TC) > 0
          THEN ROUND(X.Precio * Y.Multiplicador * (SELECT VALOR FROM TC) * (1 - ISNULL(X.Margen,0.70)), 2)
        ELSE 0
      END
    AS DECIMAL(18,2))                          AS Cost,

    -- Venta COP = Costo efectivo / (1 - Margen)
    CAST(
      (
        CASE 
          WHEN X.Costo IS NOT NULL AND X.Costo > 0 
            THEN X.Costo
          WHEN X.Precio IS NOT NULL AND X.Precio > 0 
               AND Y.Multiplicador IS NOT NULL AND Y.Multiplicador > 0
               AND (SELECT VALOR FROM TC) > 0
            THEN ROUND(X.Precio * Y.Multiplicador * (SELECT VALOR FROM TC) * (1 - ISNULL(X.Margen,0.70)), 2)
          ELSE 0
        END
      ) / NULLIF(1 - ISNULL(X.Margen,0.70),0)
    AS DECIMAL(18,2))                          AS SalePriceCop,

    -- Venta USD = Venta COP / TC
    CAST(
      (
        (
          CASE 
            WHEN X.Costo IS NOT NULL AND X.Costo > 0 
              THEN X.Costo
            WHEN X.Precio IS NOT NULL AND X.Precio > 0 
                 AND Y.Multiplicador IS NOT NULL AND Y.Multiplicador > 0
                 AND (SELECT VALOR FROM TC) > 0
              THEN ROUND(X.Precio * Y.Multiplicador * (SELECT VALOR FROM TC) * (1 - ISNULL(X.Margen,0.70)), 2)
            ELSE 0
          END
        ) / NULLIF(1 - ISNULL(X.Margen,0.70),0)
      ) / NULLIF((SELECT VALOR FROM TC),0)
    AS DECIMAL(18,2))                          AS SalePriceUsd,

    -- Multiplicadores (tabla de sublínea)
    Y.Multiplicador                             AS Multiplier,
    Y.MultiplicaDistribuidor                    AS DistributorMultiplier,

    -- Base USD: usa X.Precio si >0; si no, recalcula desde costo resultante
    CAST(
      CASE 
        WHEN X.Precio IS NOT NULL AND X.Precio > 0 
          THEN X.Precio
        ELSE
          CASE 
            WHEN (SELECT VALOR FROM TC) > 0 AND ISNULL(Y.Multiplicador,0) > 0
            THEN ROUND(
              (
                (
                  CASE 
                    WHEN X.Costo IS NOT NULL AND X.Costo > 0 
                      THEN X.Costo
                    WHEN X.Precio IS NOT NULL AND X.Precio > 0 
                         AND Y.Multiplicador IS NOT NULL AND Y.Multiplicador > 0
                         AND (SELECT VALOR FROM TC) > 0
                      THEN ROUND(X.Precio * Y.Multiplicador * (SELECT VALOR FROM TC) * (1 - ISNULL(X.Margen,0.70)), 2)
                    ELSE 0
                  END
                ) / NULLIF(1 - ISNULL(X.Margen,0.70),0)
              ) / NULLIF((SELECT VALOR FROM TC),0) / NULLIF(Y.Multiplicador,0)
            , 2)
            ELSE 0
          END
      END
    AS DECIMAL(18,2))                           AS BasePriceUsd
FROM CILAMINAIRE..MTMERCIA M
LEFT JOIN CILAMINAIRE..MTSBLIN S ON S.CODSBLIN = M.CODSBLIN
LEFT JOIN dbo.TBL_PRECIOS_BASE_EXPO X ON X.Producto = M.CODIGO
LEFT JOIN dbo.TBL_MULTIPLICADORES Y ON Y.Sublinea = M.CODSBLIN
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
        // UPSERT y updates seguros: garantiza fila en TBL_PRECIOS_BASE_EXPO y no escribe NULL en TBL_MULTIPLICADORES
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

-- Margen no-cero por defecto (por si viene 0)
DECLARE @MarginNZ DECIMAL(10,6) = CASE WHEN @Margin IS NULL OR @Margin <= 0 THEN 0.70 ELSE @Margin END;

-- 3) Calcular o usar BaseUSD
DECLARE @BaseUSD DECIMAL(18,2) =
CASE 
    WHEN @BasePriceUsd IS NOT NULL AND @BasePriceUsd > 0 
        THEN @BasePriceUsd   -- si vino del frontend, se respeta
    WHEN ISNULL(@TC,0) = 0 OR ISNULL(@Multiplier,0) = 0 
        THEN NULL
    ELSE CAST(ROUND(((@Cost / (1 - @MarginNZ)) / @TC) / @Multiplier, 2) AS DECIMAL(18,2))
END;

-- 3.5) Si no existe fila en TBL_PRECIOS_BASE_EXPO, la insertamos
DECLARE @CostIns DECIMAL(18,2) =
CASE 
  WHEN @Cost IS NOT NULL AND @Cost > 0 THEN @Cost
  WHEN @BaseUSD IS NOT NULL AND @BaseUSD > 0 
       AND @Multiplier IS NOT NULL AND @Multiplier > 0
       AND @TC IS NOT NULL AND @TC > 0
    THEN CAST(ROUND(@BaseUSD * @Multiplier * @TC * (1 - @MarginNZ), 2) AS DECIMAL(18,2))
  ELSE 0
END;

IF NOT EXISTS (SELECT 1 FROM dbo.TBL_PRECIOS_BASE_EXPO WHERE Producto = @Code)
BEGIN
    INSERT INTO dbo.TBL_PRECIOS_BASE_EXPO
        (Producto, Costo, Multiplicador, MultiplicaDistribuidor, Margen, Precio)
    VALUES
        (@Code, @CostIns, @Multiplier, @DistributorMultiplier, @MarginNZ, COALESCE(@BaseUSD,0));
END;

-- 4) Actualizar costo, multiplicadores, margen y precio (respetando valores cuando vienen NULL)
UPDATE dbo.TBL_PRECIOS_BASE_EXPO
SET Costo                  = COALESCE(NULLIF(@Cost, NULL), Costo),
    Multiplicador          = COALESCE(NULLIF(@Multiplier, NULL), Multiplicador),
    MultiplicaDistribuidor = COALESCE(NULLIF(@DistributorMultiplier, NULL), MultiplicaDistribuidor),
    Margen                 = COALESCE(NULLIF(@Margin, NULL), Margen),
    Precio                 = COALESCE(@BaseUSD, Precio)
WHERE Producto = @Code;

-- 5) Actualizar multiplicadores a nivel de Sublinea (sin escribir NULLs)
UPDATE dbo.TBL_MULTIPLICADORES
SET Multiplicador = CASE WHEN @Multiplier IS NULL THEN Multiplicador ELSE @Multiplier END,
    MultiplicaDistribuidor = CASE WHEN @DistributorMultiplier IS NULL THEN MultiplicaDistribuidor ELSE @DistributorMultiplier END
WHERE Sublinea = @SublineCode;

-- 6) Actualizar listas de precio
UPDATE MP SET PRECIO = P.Precio * P.MultiplicaDistribuidor
FROM dbo.TBL_PRECIOS_BASE_EXPO P 
INNER JOIN CILAMINAIRE..MTMERCIA M ON M.CODIGO = P.Producto 
INNER JOIN CILAMINAIRE..MVPRECIO MP ON MP.CODPRODUC = P.Producto AND MP.CODPRECIO = 'DIS-1' 
WHERE P.Producto = @Code
  AND P.MultiplicaDistribuidor IS NOT NULL 
  AND CAST(P.Precio * P.MultiplicaDistribuidor AS numeric(28,15)) <> MP.PRECIO;

UPDATE MP SET PRECIO = P.Precio * P.Multiplicador
FROM dbo.TBL_PRECIOS_BASE_EXPO P 
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
