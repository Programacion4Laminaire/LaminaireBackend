using City.Application.Dtos;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace City.Application.UseCases;

public class GetAllCityQuery : BaseFilters, IQuery<IEnumerable<CityDto>> { }