
using Identity.Application.Dtos.MenuRoles;
using Identity.Application.UseCases.MenuRoles.Commands.Upsert;
using Identity.Application.UseCases.MenuRoles.Queries.GetByRoleIdQuery;
using Identity.Application.UseCases.MenuRoles.Queries.GetTreeByRoleId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Identity
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuRoleController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher _dispatcher = dispatcher;

        /// <summary> Lista de menús (activos con Url) con flag selected para un rol. </summary>
        [HttpGet("ByRole/{roleId:int}")]
        public async Task<IActionResult> GetByRole(int roleId)
        {
            var result = await _dispatcher.Dispatch<GetMenusByRoleIdQuery, IEnumerable<MenuRoleByRoleResponseDto>>(
                new GetMenusByRoleIdQuery { RoleId = roleId }, CancellationToken.None);
            return Ok(result);
        }

        // NUEVO: árbol n-niveles
        [HttpGet("TreeByRole/{roleId:int}")]
        public async Task<IActionResult> GetTreeByRole(int roleId)
        {
            var result = await _dispatcher.Dispatch<GetMenuTreeByRoleIdQuery, IEnumerable<MenuRoleTreeResponseDto>>(
                new GetMenuTreeByRoleIdQuery { RoleId = roleId }, CancellationToken.None);
            return Ok(result);
        }


        /// <summary> Reemplaza las asignaciones Menú↔Rol de forma transaccional. </summary>
        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert([FromBody] MenuRoleUpsertRequestDto request)
        {
            var result = await _dispatcher.Dispatch<UpsertMenuRolesCommand, bool>(
                new UpsertMenuRolesCommand { Request = request }, CancellationToken.None);
            return Ok(result);
        }
    }
}
