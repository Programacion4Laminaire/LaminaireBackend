using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Permissions.Commands.UpdateCommand;

public class UpdatePermissionHandler(IUnitOfWork uow)
    : ICommandHandler<UpdatePermissionCommand, bool>
{
    public async Task<BaseResponse<bool>> Handle(UpdatePermissionCommand request, CancellationToken ct)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var entity = request.Adapt<Permission>();
            entity.Id = request.PermissionId;

            uow.Permission.UpdateAsync(entity);
            await uow.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Actualización exitosa.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
