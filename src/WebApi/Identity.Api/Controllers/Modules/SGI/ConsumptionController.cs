using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGI.Application.Dtos.Consumption;
using SGI.Application.Interfaces.Services;
using SGI.Application.UseCases.Consumption.Commands.CreateCommand;
using SGI.Application.UseCases.Consumption.Commands.DeleteCommand;
using SGI.Application.UseCases.Consumption.Commands.UpdateCommand;
using SGI.Application.UseCases.Consumption.Queries.GetAllQuery;
using SGI.Application.UseCases.Consumption.Queries.GetByIdQuery;
using SharedKernel.Abstractions.Messaging;

// 👇 agrega esta using para el atributo HasPermission
using Identity.Infrastructure.Authentication;

namespace SGI.Api.Controllers.Modules.SGI;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ConsumptionController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    /// <summary>Listado/consulta de consumos (paginado + filtros).</summary>
    [HttpGet]
    [HasPermission("consumption.view")]  
    public async Task<IActionResult> List([FromQuery] GetAllConsumptionQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllConsumptionQuery, IEnumerable<ConsumptionResponseDto>>(query, CancellationToken.None);
        return Ok(response);
    }

    /// <summary>Detalle por Id.</summary>
    [HttpGet("{consumptionId:int}")]
    [HasPermission("consumption.view")]   // Ver detalle
    public async Task<IActionResult> ById(int consumptionId)
    {
        var response = await _dispatcher.Dispatch<GetConsumptionByIdQuery, ConsumptionByIdResponseDto>(
            new GetConsumptionByIdQuery { ConsumptionId = consumptionId }, CancellationToken.None);
        return Ok(response);
    }

    /// <summary>Crear registro de consumo.</summary>
    [HttpPost("Create")]
    [HasPermission("consumption.create")]
    public async Task<IActionResult> Create([FromBody] CreateConsumptionCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateConsumptionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    /// <summary>Actualizar registro de consumo.</summary>
    [HttpPut("Update")]
    [HasPermission("consumption.update")]
    public async Task<IActionResult> Update([FromBody] UpdateConsumptionCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateConsumptionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    /// <summary>Eliminar registro de consumo.</summary>
    [HttpPut("Delete/{consumptionId:int}")]
    [HasPermission("consumption.delete")]
    public async Task<IActionResult> Delete(int consumptionId)
    {
        var response = await _dispatcher.Dispatch<DeleteConsumptionCommand, bool>(new DeleteConsumptionCommand(consumptionId), CancellationToken.None);
        return Ok(response);
    }

    /// <summary>Exportar a Excel el resultado de la consulta actual.</summary>
    [HttpGet("Export")]
    [HasPermission("consumption.export")]
    public async Task<IActionResult> Export(
        [FromQuery] GetAllConsumptionQuery query,
        [FromServices] IConsumptionExportService exportService)
    {
        var response = await _dispatcher.Dispatch<GetAllConsumptionQuery, IEnumerable<ConsumptionResponseDto>>(
            query, CancellationToken.None
        );

        if (response == null || response.Data == null || !response.Data.Any())
        {
            return NotFound("No hay consumos para exportar.");
        }

        var fileContent = exportService.ExportToExcel(response.Data);

        return File(
            fileContent,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Consumos.xlsx"
        );
    }
}
