using ResultCouriers.Application.Interfaces;
using ResultCouriers.Application.Dtos;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System.Linq;

namespace CourierJob.Application.UseCases;

public class GetCarrierJobByIdHandler(IResultCourierRepository repository) : IQueryHandler<GetResultCourierJobByIdQuery,ResultCourierDto>
{
    private readonly IResultCourierRepository _repository =repository;
   

    public async Task<BaseResponse<ResultCourierDto>> Handle(GetResultCourierJobByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<ResultCourierDto>();

        try
        {
            SharedKernel.Domain.Entities.ResultCouriers? user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            Console.WriteLine(user);
           
            response.IsSuccess = true;
            response.Data =  new ResultCourierDto
                {
                    Id = user.Id,
                    IdCourier = user.IdCourier,
                    IdCourierJob = user.IdCourierJob,
                    Price = user.Price,
                    Currency = user.Currency,
                    Service = user.Service,
                    Eta = user.Eta,
                    Status = user.Status,

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
