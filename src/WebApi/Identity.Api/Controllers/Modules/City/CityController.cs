
using City.Application.Dtos;
using MediatR; // Usamos MediatR, aunque tu IDispatcher es un wrapper
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging; // Asegúrate de que esta referencia es correcta

// Asegúrate de que los namespaces para tus Queries sean correctos
using City.Application.UseCases;
using SharedKernel.Commons.Bases;




namespace City.Api.Controllers.Modules.City;

[Route("api/[controller]")]
[ApiController]
public class CityController(IDispatcher dispatcher) : ControllerBase
{
    private readonly IDispatcher _dispatcher = dispatcher;

    [HttpPost("Create")]
    public async Task<IActionResult> CityCreate([FromBody] CreateCityCommand command)
    {
        var response = await _dispatcher.Dispatch<CreateCityCommand, bool>
           (command, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> CityList([FromQuery] Application.UseCases.GetAllCityQuery query)
    {
        var response = await _dispatcher.Dispatch<GetAllCityQuery, IEnumerable<CityDto>>
            (query, CancellationToken.None);
        return Ok(response);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> CityById(int Id)
    {
        var response = await _dispatcher.Dispatch<GetCityByIdQuery, CityDto>
            (new GetCityByIdQuery() { Id = Id }, CancellationToken.None);
        return Ok(response);
    }
    [HttpGet("ByCountry/{countryId:int}")]
    public async Task<IActionResult> GetCitiesByCountryId(int countryId, CancellationToken cancellationToken)
    {
        var query = new GetCitiesByCountryIdQuery { CountryId = countryId };

        var response = await _dispatcher.Dispatch<GetCitiesByCountryIdQuery, IEnumerable<CityDto>>(
            query,
            cancellationToken
        );

        return Ok(response);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> CityUpdate(int id, [FromBody] UpdateCityCommand request)
    {
        var command = new UpdateCityCommand
        {
            Id = id,
            Name = request.Name,
            CountryId= request.CountryId
        };

        // 2. Despachar el comando que ahora contiene toda la información
        var response = await _dispatcher.Dispatch<UpdateCityCommand, bool>(command, CancellationToken.None);



        return Ok(response);
    }


}

