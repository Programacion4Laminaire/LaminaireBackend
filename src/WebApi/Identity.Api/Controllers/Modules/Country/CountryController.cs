
using Country.Application.Dtos;
using MediatR; // Usamos MediatR, aunque tu IDispatcher es un wrapper
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging; // Asegúrate de que esta referencia es correcta

// Asegúrate de que los namespaces para tus Queries sean correctos
using Country.Application.UseCases;
using Identity.Application.Dtos.Users;
using Identity.Application.UseCases.Users.Queries.GetByIdQuery;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Identity.Application.UseCases.Users.Commands.CreateCommand;
using Identity.Application.UseCases.Users.Commands.DeleteCommand;
using Country.Application.Interfaces;


namespace Country.Api.Controllers.Modules.Country;

[Route("api/[controller]")]
[ApiController]
public class CountryController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Create")]
    public async Task<IActionResult> UserCreate([FromBody] CreateCountryCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateCountryCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> UserList([FromQuery] GetAllCountryQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllCountryQuery, IEnumerable<CountryDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> UserById(int Id)
    {
        var response = await _dispatcher.Dispatch<GetCountryByIdQuery,CountryDto>
            (new GetCountryByIdQuery() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UserUpdate(int id, [FromBody] UpdateCountryCommand request)
    {
        var command = new UpdateCountryCommand
        {
            Id = id,
            Name = request.Name
        };

        // 2. Despachar el comando que ahora contiene toda la información
        var response = await _dispatcher.Dispatch<UpdateCountryCommand, bool>(command, CancellationToken.None);

       

        return Ok(response);
    }
    [HttpDelete("Delete/{Id:int}")]
    public async Task<IActionResult> UserDelete(int Id)
    {
        var response = await _dispatcher.Dispatch<DeleteCountryCommand, bool>
            (new DeleteCountryCommand() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }
}

