using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.DeleteCommand
{

    public class DeleteHandler(IUnitOfWork uow)
        : ICommandHandler<DeleteCommand, bool>
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<BaseResponse<bool>> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var ok = await _uow.AccessoryEquivalence.DeleteAsync(request.Id);
                response.IsSuccess = ok;
                response.Data = ok;
                response.Message = ok ? "Registro eliminado correctamente." : "No se pudo eliminar el registro.";
                if (ok) await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
