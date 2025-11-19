using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Application.Dtos.ReprogramLines.Requests;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.UseCases.ReprogramLines.Queries;
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

}
