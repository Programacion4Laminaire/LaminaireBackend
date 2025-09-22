using Identity.Application.Dtos.Users;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Commands.DeleteCommand;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Identity.Application.UseCases.Users.Queries.GetAllQuery;
using Identity.Application.UseCases.Users.Queries.GetByIdQuery;
using Identity.Application.UseCases.Users.Queries.GetSelectQuery;
using Identity.Application.UseCases.Users.Queries.UserRolePermissionsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Identity.Api.Controllers.Modules.Identity;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> UserList([FromQuery] GetAllUserQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllUserQuery, IEnumerable<UserResponseDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> UserSelect()
    {
        var response = await _dispatcher
          .Dispatch<GetUserSelectQuery, IEnumerable<SelectResponseDto>>
          (new GetUserSelectQuery(), CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> UserById(int userId)
    {
        var response = await _dispatcher.Dispatch<GetUserByIdQuery, UserByIdResponseDto>
            (new GetUserByIdQuery() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("UserWithRoleAndPermissions/{userId:int}")]
    public async Task<IActionResult> UserWithRoleAndPermissions(int userId)
    {
        var response = await _dispatcher.Dispatch<GetUserWithRoleAndPermissionsQuery, UserWithRoleAndPermissionsDto>
            (new GetUserWithRoleAndPermissionsQuery() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> UserCreate([FromBody] CreateUserCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateUserCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPost("CreateWithImage")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateWithImage([FromForm] CreateUserWithImageCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateUserWithImageCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }
    [HttpPut("UpdateWithImage")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateWithImage([FromForm] UpdateUserWithImageCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateUserWithImageCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }


    [HttpPut("Update")]
    public async Task<IActionResult> UserUpdate([FromBody] UpdateUserCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateUserCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Delete/{userId:int}")]
    public async Task<IActionResult> UserDelete(int userId)
    {
        var response = await _dispatcher.Dispatch<DeleteUserCommand, bool>
            (new DeleteUserCommand() { UserId = userId }, CancellationToken.None);
        return Ok(response);
    }
}