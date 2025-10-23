using SharedKernel.Commons.Bases;
using SharedKernel.Abstractions.Messaging;
using Couriers.Application.UseCases;
using Couriers.Application.Interfaces;
using SharedKernel.Abstractions.Services;
using Couriers.Application.Dtos;
using Microsoft.EntityFrameworkCore;


// El Handler está configurado correctamente para devolver IEnumerable<CourierDto>
public class GetAllCourierQueryHandler
    : IQueryHandler<GetAllCourierQuery, IEnumerable<CourierDto>>
{
    private readonly ICourierRepository _repository;
    private readonly IOrderingQuery _orderingQuery;

    public GetAllCourierQueryHandler(ICourierRepository repository, IOrderingQuery orderingQuery)
    {
        _repository = repository;
        _orderingQuery = orderingQuery;
    }

    public async Task<BaseResponse<IEnumerable<CourierDto>>> Handle(
        GetAllCourierQuery query,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<CourierDto>>();

        try
        {
            var carrierJobsQuery = _repository.GetCourierAsQueryable();
            if (query.NumFilter is not null && !string.IsNullOrEmpty(query.TextFilter))
            {
                switch (query.NumFilter)
                {
                    case 1:
                        carrierJobsQuery = carrierJobsQuery.Where(x => x.Name.Contains(query.TextFilter));
                        break;
                }
            }


            response.TotalRecords = await carrierJobsQuery.CountAsync(cancellationToken);

            query.Sort = string.IsNullOrWhiteSpace(query.Sort) ? "Id" : query.Sort;

            var orderedItemsQuery = _orderingQuery.Ordering(query, carrierJobsQuery);

           
            var dtoArrayQuery = orderedItemsQuery
                .Select(job => new CourierDto
                {
                    Id = job.Id,
                    Name = job.Name,
                    IsActive = job.IsActive,
                    Url = job.Url,
                    RequiresAuthentication = job.RequiresAuthentication,
                    Username = job.Username,
                    Password = job.Password,
                    RpaId = job.RpaId
                });

            // 3. Ejecutar la Consulta (sobre el IQueryable<DTO>)
            var items = await dtoArrayQuery.ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.Data = items; // items es IEnumerable<CourierDto>
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = $"Error al procesar la consulta: {ex.Message}";
            response.IsSuccess = false;
        }

        return response;
    }
}