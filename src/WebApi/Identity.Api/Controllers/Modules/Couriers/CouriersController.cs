
using Country.Application.Dtos;
using MediatR; // Usamos MediatR, aunque tu IDispatcher es un wrapper
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging; // Asegúrate de que esta referencia es correcta

// Asegúrate de que los namespaces para tus Queries sean correctos

using Couriers .Application.UseCases;
using Couriers.Application.Dtos;




namespace Country.Api.Controllers.Modules.Country;

[Route("api/[controller]")]
[ApiController]
public class CouriersController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Create")]
    public async Task<IActionResult> CouriersCreate([FromBody] CreateCourierCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateCourierCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> CouriersList([FromQuery] GetAllCourierQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllCourierQuery, IEnumerable<Couriers.Application.Dtos.CourierDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> UserById(int Id)
    {
        var response = await _dispatcher.Dispatch<GetCourierByIdQuery, CourierDto>
            (new GetCourierByIdQuery() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UserUpdate(int id, [FromBody] Couriers.Application.UseCases.UpdateCouriersCommand request)
    {
        var command = new Couriers.Application.UseCases.UpdateCouriersCommand
        {
            Id = id,
            Name = request.Name,
            IsActive = request.IsActive,
            Url = request.Url,
            RequiresAuthentication = request.RequiresAuthentication,
            Username = request.Username,
            Password = request.Password,
            RpaId = request.RpaId,
          
        };

        // 2. Despachar el comando que ahora contiene toda la información
        var response = await _dispatcher.Dispatch<Couriers.Application.UseCases.UpdateCouriersCommand, bool>(command, CancellationToken.None);



        return Ok(response);
    }


}

