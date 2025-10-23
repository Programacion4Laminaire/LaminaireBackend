using CourierJob.Application.Interfaces;
using CourierJob.Application.Dtos;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System.Linq;

namespace CourierJob.Application.UseCases;

public class GetCarrierJobByIdHandler(ICourierJobRepository repository) : IQueryHandler<GetCourierJobByIdQuery,CourierJobDto>
{
    private readonly ICourierJobRepository _repository =repository;
   

    public async Task<BaseResponse<CourierJobDto>> Handle(GetCourierJobByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CourierJobDto>();

        try
        {
            SharedKernel.Domain.Entities.CourierJob? user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            Console.WriteLine(user);
           
            response.IsSuccess = true;
            response.Data = new CourierJobDto
            {
                Id = user.Id,
                JobName = user.JobName,
                OriginCityId = user.OriginCityId,
                DestinationCityId = user.DestinationCityId,
                OriginCountryId = user.OriginCountryId,
                DestinationCountryId = user.DestinationCountryId,
                Address = user.Address,
                Zipcode = user.Zipcode,
                WeightInKg = user.WeightInKg,
                HeightInCm = user.HeightInCm,
                WidthInCm = user.WidthInCm,
                LengthInCm = user.LengthInCm,
                MerchandiseValueInCop = user.MerchandiseValueInCop,
                QuotationMode = user.QuotationMode,
                OriginCityName = user.originCity.Name,
                DestinationCityName = user.destinationCity.Name,
                OriginCountryName = user.originCountry.Name,
                DestinationCountryName = user.destinationCountry.Name,
                CourierResults = user.CourierResults.Select(cr => new ResultCouriers.Application.Dtos.ResultCourierDto
                {
                    IdCourierJob = cr.IdCourierJob,
                    IdCourier = cr.IdCourier,
                    Price = cr.Price,
                    Currency = cr.Currency,
                    Service = cr.Service,
                    idCouriers = cr.Couriers.Id,
                    CouriersName = cr.Couriers.Name,
                    Status= cr.Status,
                    CouriersURL= cr.Couriers.Url,
                    CouriersUsername=cr.Couriers.Username,
                    CouriersPassword=cr.Couriers.Password,
                    CouriersIsactive = cr.Couriers.IsActive,
                    Eta=cr.Eta
                }).ToList()
            };
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
