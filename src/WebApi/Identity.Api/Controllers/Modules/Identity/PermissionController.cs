using Identity.Application.Dtos.Permissions;
using Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Identity;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PermissionController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet("PermissionByRoleId/{roleId:int}")]
    public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
    {
        var response = await _dispatcher.Dispatch<GetPermissionsByRoleIdQuery, IEnumerable<PermissionsByRoleResponseDto>>
            (new GetPermissionsByRoleIdQuery() { RoleId = roleId }, CancellationToken.None);

        return Ok(response);
    }
}