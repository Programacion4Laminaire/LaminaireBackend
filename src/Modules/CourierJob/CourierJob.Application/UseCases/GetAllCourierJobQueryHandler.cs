using CourierJob.Application.Dtos;
using CourierJob.Application.Interfaces;
using CourierJob.Application.UseCases;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// 1. CAMBIO CLAVE: El Handler ahora devuelve un IEnumerable<CourierJobDto>
public class GetAllCourierJobQueryHandler
    : IQueryHandler<GetAllCourierJobQuery, IEnumerable<CourierJobDto>>
{
    private readonly ICourierJobRepository _repository;
    private readonly IOrderingQuery _orderingQuery;

    public GetAllCourierJobQueryHandler(ICourierJobRepository repository, IOrderingQuery orderingQuery)
    {
        _repository = repository;
        _orderingQuery = orderingQuery;
    }

   
    public async Task<BaseResponse<IEnumerable<CourierJobDto>>> Handle(
        GetAllCourierJobQuery query,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<CourierJobDto>>();
        Console.WriteLine(query);
        try
        {
            var carrierJobsQuery = _repository.GetCarrierJobsAsQueryable();

            if (query.NumFilter is not null && !string.IsNullOrEmpty(query.TextFilter))
            {
                switch (query.NumFilter)
                {
                    case 1:
                       carrierJobsQuery = carrierJobsQuery.Where(x => x.JobName.Contains(query.TextFilter));
                        break;
                    case 2:
                        carrierJobsQuery = carrierJobsQuery.Where(x => x.destinationCity.Name.Contains(query.TextFilter));
                        break;
                }
            }

            query.Sort = string.IsNullOrWhiteSpace(query.Sort) ? "Id" : query.Sort;

         
            var orderedItemsQuery = _orderingQuery.Ordering(query, carrierJobsQuery);


            var dtoArrayQuery = orderedItemsQuery
                .Select(job => new CourierJobDto
                {
                    Id = job.Id,
                    JobName = job.JobName,
                    OriginCityId = job.OriginCityId,
                    DestinationCityId = job.DestinationCityId,
                    OriginCountryId = job.OriginCountryId,
                    DestinationCountryId = job.DestinationCountryId,
                    Address = job.Address,
                    Zipcode = job.Zipcode,
                    WeightInKg = job.WeightInKg,
                    HeightInCm = job.HeightInCm,
                    WidthInCm = job.WidthInCm,
                    LengthInCm = job.LengthInCm,
                    MerchandiseValueInCop = job.MerchandiseValueInCop,
                    QuotationMode = job.QuotationMode,
                    QuotationModeName = (job.QuotationMode==0) ? "El mas Rapido": "El mas Economico",
                    OriginCityName = job.originCity.Name,
                    DestinationCityName = job.destinationCity.Name,
                    OriginCountryName = job.originCountry.Name,
                    DestinationCountryName = job.destinationCountry.Name,
                    CreatedAt=job.CreatedAt,
                    CourierResults = job.CourierResults.Select(cr => new ResultCouriers.Application.Dtos.ResultCourierDto
                    {
                        IdCourierJob = cr.IdCourierJob,
                        IdCourier = cr.IdCourier,
                        Price = cr.Price,
                        Currency = cr.Currency,
                        Service = cr.Service,
                        idCouriers = cr.Couriers.Id,
                        CouriersName = cr.Couriers.Name,
                        CouriersURL = cr.Couriers.Url,
                        CouriersUsername = cr.Couriers.Username,
                        CouriersPassword = cr.Couriers.Password,
                        CouriersIsactive = cr.Couriers.IsActive,

                    }).ToList()
                });

            var items = await dtoArrayQuery.ToListAsync(cancellationToken);

            response.TotalRecords = await carrierJobsQuery.CountAsync(cancellationToken);

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