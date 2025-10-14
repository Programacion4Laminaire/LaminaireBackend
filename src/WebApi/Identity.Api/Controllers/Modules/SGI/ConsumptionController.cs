
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

namespace SGI.Api.Controllers.Modules.SGI;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ConsumptionController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] GetAllConsumptionQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllConsumptionQuery, IEnumerable<ConsumptionResponseDto>>(query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{consumptionId:int}")]
    public async Task<IActionResult> ById(int consumptionId)
    {
        var response = await _dispatcher.Dispatch<GetConsumptionByIdQuery, ConsumptionByIdResponseDto>(
            new GetConsumptionByIdQuery { ConsumptionId = consumptionId }, CancellationToken.None);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateConsumptionCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateConsumptionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateConsumptionCommand command)
    {
        var response = await _dispatcher.Dispatch<UpdateConsumptionCommand, bool>(command, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("Delete/{consumptionId:int}")]
    public async Task<IActionResult> Delete(int consumptionId)
    {
        var response = await _dispatcher.Dispatch<DeleteConsumptionCommand, bool>(new DeleteConsumptionCommand(consumptionId), CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("Export")]
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

        return File(fileContent,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Consumos.xlsx");
    }


}
