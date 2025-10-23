using Country.Application.Dtos;
using Country.Application.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Country.Application.UseCases;

public class GetAllCountryQueryHandler(
    ICountryRepository repository,
    IOrderingQuery orderingQuery) 
    : IQueryHandler<GetAllCountryQuery, IEnumerable<CountryDto>>
{
    private readonly ICountryRepository _repository = repository;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<CountryDto>>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<CountryDto>>();

        try
        {
            // Obtenemos el IQueryable de ciudades
            var cities = _repository.GetAllQueryable();

            if (request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
            {
                switch (request.NumFilter)
                {
                    case 1:
                        cities = cities.Where(x => x.Name.Contains(request.TextFilter));
                        break;
                }
            }
            request.Sort ??= "Id";

            // Aplicamos orden dinámico con IOrderingQuery
            var items = await _orderingQuery.Ordering(request, cities)
                                            .ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await cities.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<CountryDto>>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
