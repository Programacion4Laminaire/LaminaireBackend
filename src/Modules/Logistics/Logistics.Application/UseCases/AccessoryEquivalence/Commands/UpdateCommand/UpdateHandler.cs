using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.UpdateCommand
{
    public class UpdateHandler(IUnitOfWork uow)
     : ICommandHandler<UpdateCommand, bool>
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<BaseResponse<bool>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                // ✅ Validación de combinación única excluyendo el propio Id
                var already = await _uow.AccessoryEquivalence.ExistsAsync(request.CodigoPT, request.CodigoMP, request.Id);
                if (already)
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "La combinación Código PT + Código MP ya existe.";
                    return response;
                }

                var dto = request.Adapt<AccessoryEquivalenceUpdateRequestDto>();
                var ok = await _uow.AccessoryEquivalence.UpdateAsync(dto);

                response.IsSuccess = ok;
                response.Data = ok;
                response.Message = ok ? "Registro actualizado correctamente." : "No se pudo actualizar el registro.";
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
}
