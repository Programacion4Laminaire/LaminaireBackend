using Engineering.Application.Dtos.Products;
using Engineering.Application.UseCases.Products.Queries.GetByCodeQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Engineering.Api.Controllers.Modules.Engineering;

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
}
