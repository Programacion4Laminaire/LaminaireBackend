
using Identity.Application.Dtos.UserPermissions;
using Identity.Application.UseCases.UserPermissions.Commands.UpsertOverrides;
using Identity.Application.UseCases.UserPermissions.Queries.GetEffectiveByUserId;
using Identity.Application.UseCases.UserPermissions.Queries.GetOverridesByUserId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Identity
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public sealed class UserPermissionController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher _dispatcher = dispatcher;

        /// <summary>Permisos efectivos (rol ± overrides) por usuario.</summary>
        [HttpGet("Effective/{userId:int}")]
        public async Task<IActionResult> Effective(int userId)
        {
            var result = await _dispatcher.Dispatch<GetEffectivePermissionsByUserIdQuery, IEnumerable<UserPermissionByUserResponseDto>>(
                new GetEffectivePermissionsByUserIdQuery { UserId = userId }, CancellationToken.None);
            return Ok(result);
        }

        /// <summary>Overrides definidos para el usuario.</summary>
        [HttpGet("Overrides/{userId:int}")]
        public async Task<IActionResult> Overrides(int userId)
        {
            var result = await _dispatcher.Dispatch<GetOverridesByUserIdQuery, IEnumerable<UserPermissionOverrideDto>>(
                new GetOverridesByUserIdQuery { UserId = userId }, CancellationToken.None);
            return Ok(result);
        }

        /// <summary>Reemplaza overrides (grant/deny) del usuario.</summary>
        [HttpPost("Overrides/Upsert")]
        public async Task<IActionResult> Upsert([FromBody] UserPermissionOverrideUpsertRequestDto request)
        {
            var result = await _dispatcher.Dispatch<UpsertUserPermissionOverridesCommand, bool>(
                new UpsertUserPermissionOverridesCommand { Request = request }, CancellationToken.None);
            return Ok(result);
        }
    }
}
