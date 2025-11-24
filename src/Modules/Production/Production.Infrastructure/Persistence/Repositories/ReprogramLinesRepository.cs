using Dapper;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.Interfaces.Persistence;
using Production.Application.UseCases.ReprogramLines.Commands;
using Production.Application.UseCases.ReprogramLines.Queries;
using Production.Application.UseCases.ReprogramLines.Queries.GetOrderProductsQuery;
using Production.Application.UseCases.ReprogramLines.Queries.ValidateOrderExistenceQuery;
using Production.Infrastructure.Persistence.Context;
using System.Data;

namespace Production.Infrastructure.Persistence.Repositories;

public class ReprogramLinesRepository(ProductionDbContext context) : IReprogramLinesRepository
{
    private readonly ProductionDbContext _context = context;

    private const string BaseGetProgrammedLinesQuery = @"
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


    private const string DeleteProgrammedLines = @"
        DELETE ESPEJOESPE
        WHERE PEDIDO = @OrderNumber AND PRODUCTO = @ProductCode";


    private const string GetOrderProducts = @"
        SELECT LTRIM(RTRIM(MV.PRODUCTO)) AS ProductCode, LTRIM(RTRIM(MT.DESCRIPCIO)) AS ProductDescription, MV.CANTORIG AS Quantity 
        FROM MVTRADE MV INNER JOIN MTMERCIA MT ON MV.PRODUCTO = MT.CODIGO 
        WHERE MV.NRODCTO = @OrderNumber and MV.TIPODCTO = 'PD' and MV.ORIGEN = 'FAC'";


    private const string ValidateOrderExistence = @"
        SELECT T.UPAC,SUM(M.CANTIDAD) AS Quantity FROM TRADE T
        LEFT JOIN MVTRADE M ON M.NRODCTO = T.NRODCTO AND M.TIPODCTO = T.TIPODCTO AND M.ORIGEN = T.ORIGEN
        WHERE T.NRODCTO = @OrderNumber  AND T.TIPODCTO = 'PD' AND T.ORIGEN = 'FAC'
        GROUP BY T.UPAC;";

    public async Task<IEnumerable<ProgrammedLinesResponseDto>> GetProgrammedLinesByOrderAndProductAsync(GetByOrderAndProductQuery programmedLines)
    {
        ArgumentNullException.ThrowIfNull(programmedLines);
        using var connection = _context.CreateConnection;
        var sql = BaseGetProgrammedLinesQuery;

        var parameters = new DynamicParameters();
        parameters.Add("OrderNumber", programmedLines.OrderNumber);

        if (programmedLines.ProductCode != "" && programmedLines.ProductCode != null)
        {
            sql = $"{BaseGetProgrammedLinesQuery} AND E.PRODUCTO = @ProductCode ORDER BY M.CODIGO";
            parameters.Add("ProductCode", programmedLines.ProductCode);
        }

        return await connection.QueryAsync<ProgrammedLinesResponseDto>(sql, parameters);
    }

    public async Task<IEnumerable<ProgrammedLinesResponseDto>> UpdateProgrammedLinesByOrderAndProductAsync(UpdateProgrammedLinesCommand programmedLines)
    {
        ArgumentNullException.ThrowIfNull(programmedLines);
        using var connection = _context.CreateConnection;
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var deleteParams = new { programmedLines.OrderNumber, programmedLines.ProductCode };

            await connection.ExecuteAsync(DeleteProgrammedLines, deleteParams, transaction: transaction);

            var spParams = new DynamicParameters();
            spParams.Add("@pLote", programmedLines.BatchNumber);
            spParams.Add("@pPedido", programmedLines.OrderNumber);
            spParams.Add("@pCodigo", programmedLines.ProductCode);
            spParams.Add("@pUsuario", programmedLines.UserCode);

            await connection.ExecuteAsync("SPX_PROGRAMAR_LOTE", spParams, commandType: CommandType.StoredProcedure, transaction: transaction, commandTimeout: 300);

            transaction.Commit();

            return await GetProgrammedLinesByOrderAndProductAsync(
                new GetByOrderAndProductQuery
                {
                    OrderNumber = programmedLines.OrderNumber,
                    ProductCode = programmedLines.ProductCode
                }
            );
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally {

            connection.Close();
        }
    }

    public async Task<IEnumerable<OrderProductsResponseDto>> GetOrderProductsByOrderAsync(GetOrderProductsByOrderQuery Order)
    {
        ArgumentNullException.ThrowIfNull(Order);
        using var connection = _context.CreateConnection;
        var sql = GetOrderProducts;

        var parameters = new DynamicParameters();
        parameters.Add("OrderNumber", Order.OrderNumber);


        return await connection.QueryAsync<OrderProductsResponseDto>(sql, parameters);
    }

    public async Task<string> ValidateOrderExistenceAsync(ValidateOrderExistenceQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        using var connection = _context.CreateConnection;
        var sql = ValidateOrderExistence;

        var parameters = new DynamicParameters();
        parameters.Add("OrderNumber", query.OrderNumber);

        var result = await connection.QueryFirstOrDefaultAsync<OrderProductsValidationResponse>(sql, parameters);

        if (result is null)
            return "Pedido No Existe";

        if (result.UPAC)
            return "Pedido Anulado";

        if (result.Quantity == 0)
            return "Pedido Facturado";

        return "Pedido Activo";
    }

}

public class OrderProductsValidationResponse
{
    public bool UPAC { get; set; }
    public decimal Quantity { get; set; }
}
