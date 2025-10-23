using Identity.Application.Dtos.Menus;
using Identity.Application.UseCases.Menus.Commands.CreateCommand;
using Identity.Application.UseCases.Menus.Commands.DeleteCommand;
using Identity.Application.UseCases.Menus.Commands.UpdateCommand;
using Identity.Application.UseCases.Menus.Queries.GetAllQuery;
using Identity.Application.UseCases.Menus.Queries.GetByIdQuery;
using Identity.Application.UseCases.Menus.Queries.GetSelectQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;
using System.Security.Claims;

namespace Identity.Api.Controllers.Modules.Identity
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(IDispatcher dispatcher, IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IDispatcher _dispatcher = dispatcher;
        private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;

        // ====== EXISTENTE: Menú por usuario para el sidebar ======
        [HttpGet("MenuByUser")]
        public async Task<IActionResult> GetMenuByUserId()
        {
            var userId = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _dispatcher.Dispatch<GetMenuByUserIdQuery, IEnumerable<MenuResponseDto>>
                (new GetMenuByUserIdQuery() { UserId = int.Parse(userId!) }, CancellationToken.None);

            return Ok(response);
        }

        // =================== CRUD de Menús ===================

        // Listado con filtros/paginación/orden
        [HttpGet]
        public async Task<IActionResult> MenuList([FromQuery] GetAllMenuQuery query)
        {
            var response = await _dispatcher.Dispatch<GetAllMenuQuery, IEnumerable<MenuCrudResponseDto>>
                (query, CancellationToken.None);

            return Ok(response);
        }

        // Select (opcional) para combos (Id/Name)
        [HttpGet("Select")]
        public async Task<IActionResult> MenuSelect()
        {
            var response = await _dispatcher
                .Dispatch<GetMenuSelectQuery, IEnumerable<SelectResponseDto>>
                (new GetMenuSelectQuery(), CancellationToken.None);

            return Ok(response);
        }

        // Por Id (para editar)
        [HttpGet("{menuId:int}")]
        public async Task<IActionResult> MenuById(int menuId)
        {
            var response = await _dispatcher.Dispatch<GetMenuCrudByIdQuery, MenuCrudByIdResponseDto>
                (new GetMenuCrudByIdQuery { MenuId = menuId }, CancellationToken.None);

            return Ok(response);
        }

        // Crear
        [HttpPost("Create")]
        public async Task<IActionResult> MenuCreate([FromBody] CreateMenuCommand command)
        {
            var response = await _dispatcher.Dispatch<CreateMenuCommand, bool>(command, CancellationToken.None);
            return Ok(response);
        }

        // Actualizar
        [HttpPut("Update")]
        public async Task<IActionResult> MenuUpdate([FromBody] UpdateMenuCommand command)
        {
            var response = await _dispatcher.Dispatch<UpdateMenuCommand, bool>(command, CancellationToken.None);
            return Ok(response);
        }

        // Eliminar (soft-delete)
        [HttpPut("Delete/{menuId:int}")]
        public async Task<IActionResult> MenuDelete(int menuId)
        {
            var response = await _dispatcher.Dispatch<DeleteMenuCommand, bool>
                (new DeleteMenuCommand { MenuId = menuId }, CancellationToken.None);

            return Ok(response);
        }
    }
}
