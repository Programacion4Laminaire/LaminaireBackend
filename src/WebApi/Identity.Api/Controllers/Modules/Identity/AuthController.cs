using Identity.Application.UseCases.Users.Commands.LoginRefreshTokenCommand;
using Identity.Application.UseCases.Users.Commands.RevokeRefreshTokenCommand;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Identity;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery request)
    {
        var response = await _dispatcher.Dispatch<LoginQuery, string>(request, CancellationToken.None);

        if ((response.IsSuccess != null && response.IsSuccess == true) && !string.IsNullOrEmpty(response.CookieDatos))
        {
            var pretty = response.CookieDatos.Replace(" ", "+");
            Response.Cookies.Append("DatosPretty", pretty, new CookieOptions
            {
                HttpOnly = false,            // debe ser visible en DevTools y JS
                Secure = true,
                SameSite = SameSiteMode.Lax, // ajusta según tu flujo
                Expires = DateTimeOffset.UtcNow.AddHours(8),
                Path = "/"
            });

        }

        return Ok(response);
    }


    [HttpPost("LoginRefreshToken")]
    public async Task<IActionResult> LoginRefreshToken([FromBody] LoginRefreshTokenCommand request)
    {
        var response = await _dispatcher.Dispatch<LoginRefreshTokenCommand, string>(request, CancellationToken.None);
        return Ok(response);
    }

    [HttpDelete("RevokeRefreshToken/{userId:int}")]
    public async Task<IActionResult> RevokeRefreshToken(int userId)
    {
        var response = await _dispatcher.Dispatch<RevokeRefreshTokenCommand, bool>
            (new RevokeRefreshTokenCommand() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPut("UpdateUserPasswordByIdentity")]
    public async Task<IActionResult> UpdateUserPasswordByIdentity([FromBody] UpdateUserPasswordByIdentityCommand request)
    {
        var response = await _dispatcher.Dispatch<UpdateUserPasswordByIdentityCommand, bool>(request, CancellationToken.None);
        return Ok(response);
    }
}