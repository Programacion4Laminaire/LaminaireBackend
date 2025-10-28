using Couriers.Application.Interfaces;
using Couriers.Application.Dtos;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System.Linq;
using Couriers.Application.UseCases;

namespace CourierJob.Application.UseCases;

public class GetCarrierJobByIdHandler(ICourierRepository repository) : IQueryHandler<GetCourierByIdQuery,CourierDto>
{
    private readonly ICourierRepository _repository =repository;
   

    public async Task<BaseResponse<CourierDto>> Handle(GetCourierByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CourierDto>();

        try
        {
            SharedKernel.Domain.Entities.Couriers? user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            Console.WriteLine(user);
           
            response.IsSuccess = true;
            response.Data = new CourierDto
            {
                Id = user.Id,
                Name = user.Name,
                IsActive = user.IsActive,
                Url = user.Url,
                RequiresAuthentication = user.RequiresAuthentication,
                Username = user.Username,
                Password = user.Password,
                RpaId = user.RpaId
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
