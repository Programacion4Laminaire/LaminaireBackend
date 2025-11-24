using Engineering.Application.Dtos.StateChanges;
using Engineering.Application.UseCases.StateChanges.Queries.GetReport;
using Engineering.Application.UseCases.StateChanges.Queries.ExportQuery;
using Identity.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Api.Controllers.Modules.Engineering;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class StateChangeLogController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpGet("Report")]
    [HasPermission("statechangelog.view")]
    public async Task<IActionResult> GetReport(
        [FromQuery] GetStateChangeLogReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _dispatcher
            .Dispatch<GetStateChangeLogReportQuery, IEnumerable<StateChangeLogResponseDto>>(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Export")]
    [HasPermission("statechangelog.export")]
    public async Task<IActionResult> Export(
        [FromQuery] ExportStateChangeLogQuery query,
        CancellationToken ct)
    {
        var result = await _dispatcher.Dispatch<ExportStateChangeLogQuery, Stream>(query, ct);

        if (!(result.IsSuccess ?? false) || result.Data == null)
            return BadRequest(new { message = result.Message });

        return File(
            fileStream: result.Data,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileDownloadName: $"cambios_estado_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx"
        );
    }
}
