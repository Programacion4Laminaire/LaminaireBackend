using Country.Application.Dtos;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace Country.Application.UseCases;

public class GetAllCountryQuery : BaseFilters, IQuery<IEnumerable<CountryDto>> { }