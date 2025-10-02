using Mapster;
using SGI.Application.Dtos.Consumption;
using SGI.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace SGI.Application.UseCases.Consumption.Queries.GetByIdQuery;

public class GetConsumptionByIdHandler(IUnitOfWork uow)
  : IQueryHandler<GetConsumptionByIdQuery, ConsumptionByIdResponseDto>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<ConsumptionByIdResponseDto>> Handle(GetConsumptionByIdQuery request, CancellationToken ct)
    {
        var res = new BaseResponse<ConsumptionByIdResponseDto>();

        try
        {
            var ent = await _uow.Consumption.GetByIdAsync(request.ConsumptionId);
            if (ent is null)
            {
                res.IsSuccess = false;
                res.Message = "No se encontraron registros.";
                return res;
            }

            res.IsSuccess = true;
            res.Data = ent.Adapt<ConsumptionByIdResponseDto>();
            res.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            res.Message = ex.Message;
        }

        return res;
    }
}