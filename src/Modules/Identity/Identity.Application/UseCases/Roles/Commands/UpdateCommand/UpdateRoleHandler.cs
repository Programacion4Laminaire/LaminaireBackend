using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Roles.Commands.UpdateCommand;

public class UpdateRoleHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            // Solo actualiza los datos básicos del rol
            var role = request.Adapt<Role>();
            role.Id = request.RoleId;

            _unitOfWork.Role.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();

            // NO tocar permisos ni menús aquí (se gestionan en otros módulos)
            transaction.Commit();
            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
