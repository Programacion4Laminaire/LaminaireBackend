
using Identity.Application.Dtos.RolePermissions;
using Identity.Application.UseCases.RolePermissions.Commands.Upsert;
using Identity.Application.UseCases.RolePermissions.Queries.GetByRoleId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Identity
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public sealed class RolePermissionController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher _dispatcher = dispatcher;

        [HttpGet("ByRole/{roleId:int}")]
        public async Task<IActionResult> ByRole(int roleId)
        {
            var result = await _dispatcher.Dispatch<GetPermissionsByRoleIdQuery, IEnumerable<PermissionByRoleResponseDto>>(
                new GetPermissionsByRoleIdQuery { RoleId = roleId }, CancellationToken.None);
            return Ok(result);
        }

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] PermissionUpsertRequestDto request)
        {
            var result = await _dispatcher.Dispatch<UpsertRolePermissionsCommand, bool>(
                new UpsertRolePermissionsCommand { Request = request }, CancellationToken.None);
            return Ok(result);
        }
    }
}
