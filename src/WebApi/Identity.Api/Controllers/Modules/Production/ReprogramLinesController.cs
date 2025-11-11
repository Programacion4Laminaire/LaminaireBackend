using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Production.Application.Dtos.ReprogramLines.Requests;
using Production.Application.Dtos.ReprogramLines.Responses;
using Production.Application.UseCases.ReprogramLines.Queries;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Production;


[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReprogramLinesController(IDispatcher dispatcher) : Controller
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost]
    //[HasPermission("accessoryequivalence.view")]
    public async Task<IActionResult> GetProgrammedLinesByOrderAndProduct([FromBody] ProgrammedLinesRequestDto request, CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<GetByOrderAndProductQuery, ProgrammedLinesResponseDto>(
               new GetByOrderAndProductQuery { OrderNumber = request.OrderNumber, ProductCode = request.ProductCode }, ct);
        return Ok(result);
    }

}
