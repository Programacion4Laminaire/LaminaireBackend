using Engineering.Application.Dtos.Products;
using Engineering.Application.UseCases.Products.Commands.UpdateCommand;
using Engineering.Application.UseCases.Products.Queries.GetByCodeQuery;
using Engineering.Application.UseCases.Products.Queries.GetSelectQuery;
using Identity.Infrastructure.Authentication; // 👈 trae HasPermission
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Identity.Api.Controllers.Modules.Engineering;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    /// <summary>Get product details with calculated prices by product code.</summary>
    [HttpGet("{code}")]
    [HasPermission("product.view")]                // 👈 ver
    public async Task<IActionResult> GetByCode(string code, CancellationToken ct)
    {
        var response = await _dispatcher.Dispatch<GetProductByCodeQuery, ProductPriceResponseDto>(
            new GetProductByCodeQuery { Code = code }, ct);

        return Ok(response);
    }

    /// <summary>Autocomplete para productos.</summary>
    [HttpGet("Select")]
    [HasPermission("product.search")]              // 👈 buscar
    public async Task<IActionResult> GetSelect([FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Dispatch<GetProductSelectQuery, IEnumerable<SelectResponseDto>>(
            new GetProductSelectQuery(searchTerm), cancellationToken);
        return Ok(result);
    }

    /// <summary>Actualizar parámetros del producto.</summary>
    [HttpPut("Update")]
    [HasPermission("product.update")]              // 👈 actualizar
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.Dispatch<UpdateProductCommand, bool>(command, cancellationToken);
        return Ok(result);
    }
}
