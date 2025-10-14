using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.UseCases.AccessoryEquivalence.Commands.CreateCommand;
using Logistics.Application.UseCases.AccessoryEquivalence.Commands.DeleteCommand;
using Logistics.Application.UseCases.AccessoryEquivalence.Commands.UpdateCommand;
using Logistics.Application.UseCases.AccessoryEquivalence.Queries;
using Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetDescripcionQuery;
using Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetPagedQuery;
using Logistics.Application.UseCases.AccessoryEquivalence.Queries.SelectQuery;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Identity.Api.Controllers.Modules.Logistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessoryEquivalenceController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher _dispatcher = dispatcher;

        /// <summary>Listado paginado para la grilla.</summary>
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int records = 10,
            [FromQuery] string sort = "Id",
            [FromQuery] string order = "desc",
            [FromQuery] int numPage = 1,
            [FromQuery] int numFilter = 0,
            [FromQuery] string? textFilter = null,
            [FromQuery] bool download = false, // si luego quieres exportar
            CancellationToken ct = default)
        {
            var query = new GetPagedQuery
            {
                Records = records,
                Sort = sort,
                Order = order,
                NumPage = numPage,
                NumFilter = numFilter,
                TextFilter = textFilter
            };

            var result = await _dispatcher.Dispatch<GetPagedQuery, IEnumerable<AccessoryEquivalenceResponseDto>>(query, ct);
            return Ok(result); // BaseResponse con Data/TotalRecords/Message
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<GetByIdQuery, AccessoryEquivalenceResponseDto>(
                new GetByIdQuery { Id = id }, ct);
            return Ok(result);
        }

        /// <summary>Consulta la descripción por código en MTMERCIA.</summary>
        [HttpGet("Descripcion/{codigo}")]
        public async Task<IActionResult> GetDescripcion(string codigo, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<GetDescripcionQuery, string>(
                new GetDescripcionQuery { Codigo = codigo }, ct);
            return Ok(result); // BaseResponse<string>
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCommand command, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<CreateCommand, bool>(command, ct);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommand command, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<UpdateCommand, bool>(command, ct);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<DeleteCommand, bool>(new DeleteCommand { Id = id }, ct);
            return Ok(result);
        }
        /// <summary>Autocomplete a MTMERCIA. kind: PT | MP (opcional).</summary>
        [HttpGet("Select")]
        public async Task<IActionResult> GetSelect([FromQuery] string? searchTerm, [FromQuery] string? kind, CancellationToken ct)
        {
            var result = await _dispatcher.Dispatch<GetMerciaSelectQuery, IEnumerable<SelectResponseDto>>(
                new GetMerciaSelectQuery { SearchTerm = searchTerm, Kind = kind }, ct);
            return Ok(result);
        }


    }
}
