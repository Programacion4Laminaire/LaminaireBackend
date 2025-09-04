﻿using Identity.Application.UseCases.Users.Commands.LoginRefreshTokenCommand;
using Identity.Application.UseCases.Users.Commands.RevokeRefreshTokenCommand;
using Identity.Application.UseCases.Users.Queries.LoginQuery;
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
            Response.Cookies.Append("Datos", response.CookieDatos, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
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
}