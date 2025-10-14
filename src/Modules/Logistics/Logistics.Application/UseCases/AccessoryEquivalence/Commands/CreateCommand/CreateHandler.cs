using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Mapster;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.CreateCommand;

public class CreateHandler(IUnitOfWork uow)
    : ICommandHandler<CreateCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            // ✅ Validación de combinación única
            var already = await _uow.AccessoryEquivalence.ExistsAsync(request.CodigoPT, request.CodigoMP, null);
            if (already)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = "La combinación Código PT + Código MP ya existe.";
                return response;
            }

            var dto = request.Adapt<AccessoryEquivalenceCreateRequestDto>();
            var ok = await _uow.AccessoryEquivalence.CreateAsync(dto);

            response.IsSuccess = ok;
            response.Data = ok;
            response.Message = ok ? "Registro creado correctamente." : "No se pudo crear el registro.";

            if (ok) await _uow.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Data = false;
            response.Message = ex.Message;
        }
        return response;
    }
}
