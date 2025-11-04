using Identity.Application.Dtos.Permissions;
using Identity.Application.UseCases.Permissions.Commands.CreateCommand;
using Identity.Application.UseCases.Permissions.Commands.DeleteCommand;
using Identity.Application.UseCases.Permissions.Commands.UpdateCommand;
using Identity.Application.UseCases.Permissions.Queries.GetAllQuery;
using Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;
using Identity.Application.UseCases.Permissions.Queries.GetSelectQuery;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;
using System.Security.Claims;

namespace Identity.Api.Controllers.Modules.Identity;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    private readonly IPermissionService _permissionService;

    public PermissionController(
        IDispatcher dispatcher,
        IPermissionService permissionService)
    {
        _dispatcher = dispatcher;
        _permissionService = permissionService;
    }

    // ================== NUEVO ==================
    /// <summary>Devuelve los slugs efectivos (Role ± UserPermission) del usuario autenticado.</summary>
    [HttpGet("Mine")]
    public async Task<IActionResult> Mine(CancellationToken ct)
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var userId))
            return Unauthorized();

        var data = await _permissionService.GetEffectivePermissionSlugsAsync(userId, ct);

        var resp = new BaseResponse<IEnumerable<string>>
        {
            IsSuccess = true,
            Message = GlobalMessages.MESSAGE_QUERY,
            Data = data
        };
        return Ok(resp);
    }
    // ===========================================

    // ======= EXISTENTE =======
    [HttpGet("PermissionByRoleId/{roleId:int}")]
    public async Task<IActionResult> GetPermissionsByRoleId(int roleId)
    {
        var response = await _dispatcher.Dispatch<GetPermissionsByRoleIdQuery, IEnumerable<PermissionsByRoleResponseDto>>
            (new GetPermissionsByRoleIdQuery() { RoleId = roleId }, CancellationToken.None);

        return Ok(response);
    }

    // ======= CRUD =======
    [HttpGet]
    public async Task<IActionResult> PermissionList([FromQuery] GetAllPermissionQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllPermissionQuery, IEnumerable<PermissionCrudResponseDto>>
            (query, CancellationToken.None);

        return Ok(response);
    }

    [HttpGet("Select")]
    public async Task<IActionResult> PermissionSelect()
    {
        var response = await _dispatcher.Dispatch<GetPermissionSelectQuery, IEnumerable<SelectResponseDto>>
            (new GetPermissionSelectQuery(), CancellationToken.None);

        return Ok(response);
    }

    [HttpGet("{permissionId:int}")]
    public async Task<IActionResult> PermissionById(int permissionId)
    {
        var response = await _dispatcher.Dispatch<GetPermissionByIdQuery, PermissionCrudByIdResponseDto>
            (new GetPermissionByIdQuery() { PermissionId = permissionId }, CancellationToken.None);

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> PermissionCreate([FromBody] CreatePermissionCommand command)
    {
        var response = await _dispatcher.Dispatch<CreatePermissionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> PermissionUpdate([FromBody] UpdatePermissionCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdatePermissionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Delete/{permissionId:int}")]
    public async Task<IActionResult> PermissionDelete(int permissionId)
    {
        var response = await _dispatcher.Dispatch<DeletePermissionCommand, bool>
            (new DeletePermissionCommand() { PermissionId = permissionId }, CancellationToken.None);

        return Ok(response);
    }
}
