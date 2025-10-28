using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Permissions.Commands.CreateCommand;

public class CreatePermissionHandler(IUnitOfWork uow)
    : ICommandHandler<CreatePermissionCommand, bool>
{
    public async Task<BaseResponse<bool>> Handle(CreatePermissionCommand request, CancellationToken ct)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var entity = request.Adapt<Permission>();
            await uow.Permission.CreateAsync(entity);
            await uow.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Registro exitoso.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
