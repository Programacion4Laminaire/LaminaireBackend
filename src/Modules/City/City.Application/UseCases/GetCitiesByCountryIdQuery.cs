using City.Application.Dtos;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace City.Application.UseCases;

public class GetCitiesByCountryIdQuery : IQuery<IEnumerable<CityDto>>
{
    public int CountryId { get; set; }
}
