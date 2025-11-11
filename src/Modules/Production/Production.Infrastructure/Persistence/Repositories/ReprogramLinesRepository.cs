using Dapper;
using Microsoft.IdentityModel.Tokens;
using Production.Application.Dtos.ReprogramLines.Requests;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using Production.Infrastructure.Persistence.Context;

namespace Production.Infrastructure.Persistence.Repositories;

public class ReprogramLinesRepository(ProductionDbContext context) : IReprogramLinesRepository
{

    private readonly ProductionDbContext _context = context;

    private const string BaseQuery = @"
        SELECT 
            E.ID AS Id,
            E.PEDIDO AS [Order],
            E.LOTE AS [Batch],
            M.CODIGO AS [ProductCode],
            M.DESCRIPCIO AS [ProductDescription],
            E.LINEASIR AS [Line],
            E.USP AS [Usp],
            CAST(E.FECHA AS DATE) AS [Date],
            CAST(E.FECHA AS TIME) AS [Time],
            E.USUARIO AS [User]
        FROM ESPEJOESPE E
        INNER JOIN MTMERCIA M ON E.PRODUCTO = M.CODIGO
        WHERE E.PEDIDO = @OrderNumber";

    public async Task<IEnumerable<ProgrammedLinesResponseDto>> GetMerciaSelectAsync(ProgrammedLinesRequestDto programmedLines)
    {
        ArgumentNullException.ThrowIfNull(programmedLines);

        using var connection = _context.CreateConnection;

        var sql = programmedLines.ProductCode.IsNullOrEmpty() ? BaseQuery : $"{BaseQuery} AND E.PRODUCTO = @ProductCode";

        var parameters = new
        {
            programmedLines.OrderNumber,
            programmedLines.ProductCode
        };

        return await connection.QueryAsync<ProgrammedLinesResponseDto>(sql, parameters);
    }
}
