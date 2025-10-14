using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries;

public class GetByIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetByIdQuery, AccessoryEquivalenceResponseDto>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<AccessoryEquivalenceResponseDto>> Handle(
        GetByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<AccessoryEquivalenceResponseDto>();

        try
        {
            var item = await _uow.AccessoryEquivalence.GetByIdAsync(request.Id);

            if (item is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró el registro.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = item;
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
