using Engineering.Application.Dtos.Products;
using Engineering.Application.UseCases.Products.Commands.UpdateCommand;
using Engineering.Application.UseCases.Products.Queries.GetByCodeQuery;

using Engineering.Application.UseCases.Products.Queries.GetSelectQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Dtos.Commons;

namespace Identity.Api.Controllers.Modules.Engineering;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    /// <summary>
    /// Get product details with calculated prices by product code.
    /// </summary>
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var response = await _dispatcher.Dispatch<GetProductByCodeQuery, ProductPriceResponseDto>(
            new GetProductByCodeQuery { Code = code },
            CancellationToken.None
        );

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> GetSelect([FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Dispatch<GetProductSelectQuery, IEnumerable<SelectResponseDto>>(
            new GetProductSelectQuery(searchTerm),
            cancellationToken
        );
        return Ok(result);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Dispatch<UpdateProductCommand, bool>(command, cancellationToken);
        return Ok(result);
    }


}
