using Dapper;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using Production.Application.UseCases.ReprogramLines.Queries;
using Production.Infrastructure.Persistence.Context;

namespace Production.Infrastructure.Persistence.Repositories;

public class ReprogramLinesRepository(ProductionDbContext context) : IReprogramLinesRepository
{

    private readonly ProductionDbContext _context = context;

    private const string BaseQuery = @"
        SELECT 
            E.ID AS Id,
            E.LOTE AS [BatchNumber],
            E.PEDIDO AS [OrderNumber],
            LTRIM(RTRIM(M.CODIGO)) AS [ProductCode],
            LTRIM(RTRIM(M.DESCRIPCIO)) AS [ProductDescription],
            E.CANTIDAD AS [Quantity],
            E.LINEASIR AS [Line],
            E.USP AS [Usp],
            CAST(E.FECHA AS DATE) AS [Date],
            LTRIM(RTRIM(E.USUARIO)) AS [UserCode]
        FROM ESPEJOESPE E
        INNER JOIN MTMERCIA M ON E.PRODUCTO = M.CODIGO
        WHERE E.PEDIDO = @OrderNumber";

    public async Task<IEnumerable<ProgrammedLinesResponseDto>> GetProgrammedLinesByOrderAndProductAsync(GetByOrderAndProductQuery programmedLines)
    {
        ArgumentNullException.ThrowIfNull(programmedLines);

        using var connection = _context.CreateConnection;

        var sql = BaseQuery;

        var parameters = new DynamicParameters();
        parameters.Add("OrderNumber", programmedLines.OrderNumber);

        if (programmedLines.ProductCode != "" && programmedLines.ProductCode != null) {

            sql = $"{BaseQuery} AND E.PRODUCTO = @ProductCode ORDER BY M.CODIGO";
            parameters.Add("ProductCode", programmedLines.ProductCode);
        }


        return await connection.QueryAsync<ProgrammedLinesResponseDto>(sql, parameters);
    }
}
