using Dapper;
using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Persistence;
using Logistics.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using SharedKernel.Dtos.Commons;
using System.Text;

namespace Logistics.Infrastructure.Persistence.Repositories;

public class AccessoryEquivalenceRepository(LogisticsDbContext context) : IAccessoryEquivalenceRepository
{
    private readonly LogisticsDbContext _context = context;

    // Clase interna para mapear también el TotalRecords
    private sealed class EqRow
    {
        public int Id { get; init; }
        public string CodigoPT { get; init; } = default!;
        public string DescripcionPT { get; init; } = default!;
        public string CodigoMP { get; init; } = default!;
        public string DescripcionMP { get; init; } = default!;
        public decimal Costo { get; init; }
        public DateTime FechaCreacion { get; init; }
        public int TotalRecords { get; init; }
    }

    public async Task<(IEnumerable<AccessoryEquivalenceResponseDto> Items, int TotalRecords)> GetPagedAsync(
        int records, int numPage, string sort, string order, int numFilter, string? textFilter)
    {
        string sortColumn = (sort ?? string.Empty).Trim().ToLowerInvariant() switch
        {
            "id" => "Id",
            "codigopt" => "CodigoPT",
            "codigomp" => "CodigoMP",
            "costo" => "Costo",
            "fechacreacion" => "FechaCreacion",
            _ => "Id"
        };
        string sortDir = (order ?? "DESC").ToUpperInvariant() == "ASC" ? "ASC" : "DESC";

        var sbWhere = new StringBuilder("WHERE 1=1 ");
        var p = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(textFilter) && numFilter > 0)
        {
            p.Add("@Text", $"%{textFilter.Trim()}%");
            switch (numFilter)
            {
                case 1: sbWhere.Append("AND E.CodigoPT LIKE @Text "); break; // Código PT
                case 2: sbWhere.Append("AND E.CodigoMP LIKE @Text "); break; // Código MP
                case 3: sbWhere.Append("AND (MPT.DESCRIPCIO LIKE @Text OR MMP.DESCRIPCIO LIKE @Text) "); break; // Descripción
            }
        }

        int offset = (numPage <= 1) ? 0 : (numPage - 1) * records;
        p.Add("@Offset", offset);
        p.Add("@Records", records);

        var sql = $@"
;WITH EQ AS (
    SELECT 
        E.Id,
        LTRIM(RTRIM(E.CodigoPT))           AS CodigoPT,
        LTRIM(RTRIM(MPT.DESCRIPCIO))       AS DescripcionPT,
        LTRIM(RTRIM(E.CodigoMP))           AS CodigoMP,
        LTRIM(RTRIM(MMP.DESCRIPCIO))       AS DescripcionMP,
        CAST(E.Costo AS DECIMAL(18,6))     AS Costo,
        E.FechaCreacion,
        COUNT(1) OVER()                    AS TotalRecords
    FROM CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS E
    LEFT JOIN CILAMINAIRE..MTMERCIA MPT ON MPT.CODIGO = E.CodigoPT
    LEFT JOIN CILAMINAIRE..MTMERCIA MMP ON MMP.CODIGO = E.CodigoMP
    {sbWhere}
)
SELECT *
FROM EQ
ORDER BY {sortColumn} {sortDir}
OFFSET (@Offset) ROWS FETCH NEXT @Records ROWS ONLY;";

        using var cn = _context.CreateConnection;
        var rows = await cn.QueryAsync<EqRow>(sql, p);

        var list = rows
            .Select(r => new AccessoryEquivalenceResponseDto
            {
                Id = r.Id,
                CodigoPT = r.CodigoPT,
                DescripcionPT = r.DescripcionPT,
                CodigoMP = r.CodigoMP,
                DescripcionMP = r.DescripcionMP,
                Costo = r.Costo,
                FechaCreacion = r.FechaCreacion
            })
            .ToList();

        int total = rows.FirstOrDefault()?.TotalRecords ?? 0;
        return (list, total);
    }

    public async Task<AccessoryEquivalenceResponseDto?> GetByIdAsync(int id)
    {
        var sql = @"
SELECT 
    E.Id,
    LTRIM(RTRIM(E.CodigoPT))       AS CodigoPT,
    LTRIM(RTRIM(MPT.DESCRIPCIO))   AS DescripcionPT,
    LTRIM(RTRIM(E.CodigoMP))       AS CodigoMP,
    LTRIM(RTRIM(MMP.DESCRIPCIO))   AS DescripcionMP,
    CAST(E.Costo AS DECIMAL(18,6)) AS Costo,
    E.FechaCreacion
FROM CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS E
LEFT JOIN CILAMINAIRE..MTMERCIA MPT ON MPT.CODIGO = E.CodigoPT
LEFT JOIN CILAMINAIRE..MTMERCIA MMP ON MMP.CODIGO = E.CodigoMP
WHERE E.Id = @Id;";

        using var cn = _context.CreateConnection;
        return await cn.QueryFirstOrDefaultAsync<AccessoryEquivalenceResponseDto>(sql, new { Id = id });
    }

    public async Task<bool> CreateAsync(AccessoryEquivalenceCreateRequestDto dto)
    {
        var sql = @"
INSERT INTO CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS
    (CodigoPT, CodigoMP, Costo, FechaCreacion)
VALUES
    (@CodigoPT, @CodigoMP, @Costo, GETDATE());";

        using var cn = _context.CreateConnection;
        try
        {
            var rows = await cn.ExecuteAsync(sql, dto);
            return rows > 0;
        }
        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
        {
            // Índice único violado
            throw new InvalidOperationException("La combinación Código PT + Código MP ya existe.", ex);
        }
    }

    public async Task<bool> UpdateAsync(AccessoryEquivalenceUpdateRequestDto dto)
    {
        var sql = @"
UPDATE CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS
SET CodigoPT = @CodigoPT,
    CodigoMP = @CodigoMP,
    Costo    = @Costo
WHERE Id = @Id;";

        using var cn = _context.CreateConnection;
        try
        {
            var rows = await cn.ExecuteAsync(sql, dto);
            return rows > 0;
        }
        catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
        {
            // Índice único violado
            throw new InvalidOperationException("La combinación Código PT + Código MP ya existe.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = @"DELETE FROM CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS WHERE Id = @Id;";
        using var cn = _context.CreateConnection;
        var rows = await cn.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<string?> GetDescripcionByCodigoAsync(string codigo)
    {
        var sql = @"
SELECT TOP 1 LTRIM(RTRIM(DESCRIPCIO))
FROM CILAMINAIRE..MTMERCIA
WHERE CODIGO = @Codigo;";

        using var cn = _context.CreateConnection;
        return await cn.QueryFirstOrDefaultAsync<string>(sql, new { Codigo = codigo.Trim() });
    }

    public async Task<IEnumerable<SelectResponseDto>> GetMerciaSelectAsync(string? searchTerm, string? kind)
    {
        // Normalizamos: dígitos para el match por código y término libre para descripción
        var termRaw = (searchTerm ?? string.Empty).Trim();
        var termCode = new string(termRaw.Where(char.IsDigit).ToArray()); // para CODIGO (prefijo)
        var termText = $"%{termRaw}%";                                   // para DESCRIPCIO (contains)
        var kindUp = string.IsNullOrWhiteSpace(kind) ? null : kind.Trim().ToUpperInvariant();

        var p = new DynamicParameters();
        p.Add("@TermCode", termCode);
        p.Add("@TermText", termText);
        p.Add("@Kind", kindUp);

        // Nota: evitamos TRY_CONVERT(BIGINT) por overflow; usamos patrón de "solo dígitos".
        var sql = @"
SELECT TOP (20)
    LTRIM(RTRIM(CODIGO))     AS [Code],
    LTRIM(RTRIM(DESCRIPCIO)) AS [Description]
FROM CILAMINAIRE..MTMERCIA WITH (NOLOCK)
WHERE
    (
        -- Si hay término numérico, hacemos prefijo por código
        (@TermCode <> '' AND CODIGO LIKE @TermCode + '%')
        OR
        -- También permitimos búsqueda por descripción si escribió letras
        (@TermCode = '' AND @TermText <> '%%' AND DESCRIPCIO LIKE @TermText)
    )
    AND (
        @Kind IS NULL
        OR (
            @Kind = 'PT'
            AND LEN(RTRIM(CODIGO)) >= 15
            AND RTRIM(CODIGO) NOT LIKE '%[^0-9]%'   -- solo dígitos (sin overflow)
        )
        OR (
            @Kind = 'MP'
            AND LEN(RTRIM(CODIGO)) = 5
            AND RTRIM(CODIGO) NOT LIKE '%[^0-9]%'
        )
    )
ORDER BY CODIGO;";

        using var cn = _context.CreateConnection;
        var items = await cn.QueryAsync<SelectResponseDto>(sql, p);
        return items;
    }

    public async Task<bool> ExistsAsync(string codigoPT, string codigoMP, int? excludeId = null)
    {
        var sql = @"
SELECT TOP 1 1
FROM CILAMINAIRE..TBL_EQUIVALENCIAS_LAMIACCESORIOS
WHERE CodigoPT = @CodigoPT
  AND CodigoMP = @CodigoMP
  AND (@ExcludeId IS NULL OR Id <> @ExcludeId);";

        using var cn = _context.CreateConnection;
        var exists = await cn.ExecuteScalarAsync<int?>(
            sql,
            new
            {
                CodigoPT = (codigoPT ?? string.Empty).Trim(),
                CodigoMP = (codigoMP ?? string.Empty).Trim(),
                ExcludeId = excludeId
            });

        return exists.HasValue;
    }
}
