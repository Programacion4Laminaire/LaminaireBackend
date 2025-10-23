using City.Application.Dtos;
using City.Application.Interfaces;
using SharedKernel.Abstractions.Messaging;
using Mapster;
using City.Application.UseCases;
using SharedKernel.Commons.Bases;
using City.Application.Interfaces;
using City.Application.Dtos;
using MediatR;
using SharedKernel.Abstractions.Services;
using Microsoft.EntityFrameworkCore;



// 1. La clase implementa la interfaz IQueryHandler
//    que especifica el tipo de consulta y el tipo de resultado.
public class GetAllCityQueryHandler : IQueryHandler<GetAllCityQuery, IEnumerable<CityDto>>
{
    
    private readonly ICityRepository _repository;
    private readonly IOrderingQuery _orderingQuery;

    public GetAllCityQueryHandler(ICityRepository repository, IOrderingQuery orderingQuery)
    {
        _repository = repository;
        _orderingQuery = orderingQuery;
    }

    
    public async Task<IEnumerable<CityDto>> HandleAsync(GetAllCityQuery query)
    {
       
        var countries = await _repository.GetAllAsync();

 
        return countries.Adapt<IEnumerable<CityDto>>();
    }

    public async Task<BaseResponse<IEnumerable<CityDto>>> Handle(GetAllCityQuery query, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<CityDto>>();

        try
        {
            // Obtenemos el IQueryable de ciudades
            var cities = _repository.GetAllQueryable();


            if (query.NumFilter is not null && !string.IsNullOrEmpty(query.TextFilter))
            {
                switch (query.NumFilter)
                {
                    case 1:
                        cities = cities.Where(x => x.Name.Contains(query.TextFilter));
                        break;
                }
            }

            query.Sort ??= "Id";

            // Aplicamos orden dinámico con IOrderingQuery
            var items = await _orderingQuery.Ordering(query, cities)
                                            .ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.TotalRecords = await cities.CountAsync(cancellationToken);
            response.Data = items.Adapt<IEnumerable<CityDto>>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}