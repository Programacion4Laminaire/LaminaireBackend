using City.Application.Dtos;
using City.Application.Interfaces;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Mapster;

namespace City.Application.UseCases;

public class GetCitiesByCountryIdQueryHandler
    : IQueryHandler<GetCitiesByCountryIdQuery, IEnumerable<CityDto>>
{
    private readonly ICityRepository _repository;

    public GetCitiesByCountryIdQueryHandler(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<IEnumerable<CityDto>>> Handle(GetCitiesByCountryIdQuery query, CancellationToken cancellationToken)
    {
        var cities = await _repository.GetByCountryIdAsync(query.CountryId);

        var citiesDto = cities.Adapt<IEnumerable<CityDto>>();

        return new BaseResponse<IEnumerable<CityDto>>
        {
            Data = citiesDto,
            IsSuccess = true,
            Message = citiesDto.Any()
                ? "Ciudades encontradas."
                : "No hay ciudades para este país."
        };
    }
}
