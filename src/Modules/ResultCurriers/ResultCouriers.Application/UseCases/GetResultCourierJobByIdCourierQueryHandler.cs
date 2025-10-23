using SharedKernel.Commons.Bases;
using SharedKernel.Abstractions.Messaging;
using ResultCouriers.Application.UseCases;
using ResultCouriers.Application.Interfaces;
using SharedKernel.Abstractions.Services;
using ResultCouriers.Application.Dtos;
using Microsoft.EntityFrameworkCore;


// El Handler está configurado correctamente para devolver IEnumerable<CourierDto>
public class GetResultCourierJobByIdCourierQueryHandler
    : IQueryHandler<GetResultCourierJobByIdCourierQuery, IEnumerable<ResultCourierDto>>
{
    private readonly IResultCourierRepository _repository;
    private readonly IOrderingQuery _orderingQuery;

    public GetResultCourierJobByIdCourierQueryHandler(IResultCourierRepository repository, IOrderingQuery orderingQuery)
    {
        _repository = repository;
        _orderingQuery = orderingQuery;
    }

    public async Task<BaseResponse<IEnumerable<ResultCourierDto>>> Handle(
       GetResultCourierJobByIdCourierQuery query,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<ResultCourierDto>>();

        try
        {
            var carrierJobsQuery = _repository.GetResultCouriersAsQueryable();
            carrierJobsQuery =carrierJobsQuery.Where(j => j.IdCourier == query.IdCourier);

           
          
            response.TotalRecords = await carrierJobsQuery.CountAsync(cancellationToken);

            query.Sort = string.IsNullOrWhiteSpace(query.Sort) ? "Id" : query.Sort;

            var orderedItemsQuery = _orderingQuery.Ordering(query, carrierJobsQuery);

        
            var dtoArrayQuery = orderedItemsQuery
                .Select(job => new ResultCourierDto
                {
                    Id = job.Id,
                    IdCourier = job.IdCourier,
                    IdCourierJob = job.IdCourierJob,
                    Price = job.Price,
                    Currency = job.Currency,
                    Service = job.Service,
                    Eta = job.Eta,
                    Status = job.Status,

                });

         
            var items = await dtoArrayQuery.ToListAsync(cancellationToken);

            response.IsSuccess = true;
            response.Data = items;
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