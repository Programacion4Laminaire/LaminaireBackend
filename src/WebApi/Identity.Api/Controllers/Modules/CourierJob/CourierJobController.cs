
using Country.Application.Dtos;
using MediatR; 
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Domain.Enums; 


using CourierJob.Application.UseCases;
using CourierJob.Application.Dtos;

namespace Country.Api.Controllers.Modules.Country;

[Route("api/[controller]")]
[ApiController]
public class CourierJobController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Create")]
    public async Task<IActionResult> UserCreate([FromBody] CreateCourierJobCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateCourierJobCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> UserList([FromQuery] GetAllCourierJobQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllCourierJobQuery, IEnumerable<CourierJob.Application.Dtos.CourierJobDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> UserById(int Id)
    {
        var response = await _dispatcher.Dispatch<GetCourierJobByIdQuery,CourierJobDto>
            (new GetCourierJobByIdQuery() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }

   
}

