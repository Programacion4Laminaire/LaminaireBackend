
using CourierJob.Application.Dtos;
using CourierJob.Application.UseCases;
using Couriers.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using ResultCouriers.Application.Dtos;
using ResultCouriers.Application.UseCases;
using SharedKernel.Abstractions.Messaging;

// Asegúrate de que los namespaces para tus Queries sean correctos




namespace Country.Api.Controllers.Modules.Country;

[Route("api/[controller]")]
[ApiController]
public class ResultCouriersController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Create")]
    public async Task<IActionResult> ResultCourierCreate([FromBody]  CreateResultCourierCommand  command)
    {
        var response = await _dispatcher.Dispatch<CreateResultCourierCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }
    [HttpPost("SendResultCourier")]
    public async Task<IActionResult> ResultCourierSend([FromBody] SendResultCourierCommand command)
    {
        var response = await _dispatcher.Dispatch<SendResultCourierCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> ResultCourierList([FromQuery] GetAllResultCourierQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllResultCourierQuery, IEnumerable<ResultCouriers.Application.Dtos.ResultCourierDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> UserById(int Id)
    {
        var response = await _dispatcher.Dispatch<GetResultCourierJobByIdQuery, ResultCourierDto>
            (new GetResultCourierJobByIdQuery() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet("idCourier/{IdCourier:int}")]
    public async Task<IActionResult> ResultCourierbyIdCourierList(int IdCourier)
    {
        var query = new GetResultCourierJobByIdCourierQuery(IdCourier);
        var response = await _dispatcher.Dispatch<GetResultCourierJobByIdCourierQuery, IEnumerable<ResultCouriers.Application.Dtos.ResultCourierDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }
    

}

