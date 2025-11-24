using Microsoft.AspNetCore.Mvc;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.UseCases.ReprogramLines.Commands;
using Production.Application.UseCases.ReprogramLines.Queries;
using Production.Application.UseCases.ReprogramLines.Queries.GetOrderProductsQuery;
using Production.Application.UseCases.ReprogramLines.Queries.ValidateOrderExistenceQuery;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Production;


//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReprogramLinesController(IDispatcher dispatcher) : Controller
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet("/ProgrammedLinesByOrderAndProduct/{orderNumber}/{productCode}")]
    //[HasPermission("accessoryequivalence.view")]
    public async Task<IActionResult> GetProgrammedLinesByOrderAndProduct(string orderNumber, string productCode,
    CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<GetByOrderAndProductQuery, IEnumerable<ProgrammedLinesResponseDto>>(
        new GetByOrderAndProductQuery
        {
            OrderNumber = orderNumber,
            ProductCode = productCode
        }, ct);
        return Ok(result);
    }

    [HttpGet("/ProgrammedLinesByOrder/{orderNumber}")]
    //[HasPermission("accessoryequivalence.view")]
    public async Task<IActionResult> GetProgrammedLinesByOrder(string orderNumber,CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<GetByOrderAndProductQuery, IEnumerable<ProgrammedLinesResponseDto>>(
        new GetByOrderAndProductQuery
        {
            OrderNumber = orderNumber
        }, ct);
        return Ok(result);
    }

    [HttpGet("/OrderProductsByOrder/{orderNumber}")]
    //[HasPermission("accessoryequivalence.view")]
    public async Task<IActionResult> GetOrderProductsByOrder(string orderNumber, CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<GetOrderProductsByOrderQuery, IEnumerable<OrderProductsResponseDto>>(
        new GetOrderProductsByOrderQuery
        {
            OrderNumber = orderNumber
        }, ct);
        return Ok(result);
    }

    [HttpGet ("/ValidateOrderExistence/{OrderNumber}")]
    //[HasPermission("accessoryequivalence.view")]
    public async Task<IActionResult> ValidateOrderExistence(string OrderNumber, CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<ValidateOrderExistenceQuery, string>(
        new ValidateOrderExistenceQuery
        {
            OrderNumber = OrderNumber
        }, ct);
        return Ok(result);
    }




    [HttpPut("/UpdateProgrammedLinesByOrderAndProduct")]
    //[HasPermission("accessoryequivalence.edit")]
    public async Task<IActionResult> UpdateProgrammedLinesByOrderAndProduct([FromBody] UpdateProgrammedLinesCommand request, CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<UpdateProgrammedLinesCommand, IEnumerable<ProgrammedLinesResponseDto>>(
        new UpdateProgrammedLinesCommand
        {
            OrderNumber = request.OrderNumber,
            ProductCode = request.ProductCode,
            BatchNumber = request.BatchNumber,
            UserCode = request.UserCode

        }, ct);
        return Ok(result);
    }

}
