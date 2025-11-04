using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Permissions.Commands.DeleteCommand;

public class DeletePermissionHandler(IUnitOfWork uow)
    : ICommandHandler<DeletePermissionCommand, bool>
{
    public async Task<BaseResponse<bool>> Handle(DeletePermissionCommand request, CancellationToken ct)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var entity = await uow.Permission.GetByIdAsync(request.PermissionId);
            if (entity is null)
            {
                response.IsSuccess = false;
                response.Message = "El permiso no existe en la base de datos.";
                return response;
            }

            await uow.Permission.DeleteAsync(request.PermissionId);
            await uow.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Eliminación exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
