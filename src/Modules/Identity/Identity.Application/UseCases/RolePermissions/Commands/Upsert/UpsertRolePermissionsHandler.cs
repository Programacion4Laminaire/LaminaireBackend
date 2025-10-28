
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.RolePermissions.Commands.Upsert;

public sealed class UpsertRolePermissionsHandler(IUnitOfWork uow)
    : ICommandHandler<UpsertRolePermissionsCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(UpsertRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        using var tx = _uow.BeginTransaction();
        try
        {
            var roleId = request.Request.RoleId;

            var current = await _uow.Permission.GetRolePermissionsByRoleIdAsync(roleId);
            if (current.Count > 0)
            {
                var removed = await _uow.Permission.DeleteRolePermissionsAsync(current);
                if (!removed) throw new InvalidOperationException("No fue posible limpiar permisos del rol.");
            }

            if (request.Request.PermissionIds?.Any() == true)
            {
                var entities = request.Request.PermissionIds.Select(pid => new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = pid,
                    State = "1",
                    AuditCreateUser = 1,
                    AuditCreateDate = DateTime.Now
                });

                var created = await _uow.Permission.RegisterRolePermissionsAsync(entities);
                if (!created) throw new InvalidOperationException("No fue posible registrar permisos del rol.");
            }

            await _uow.SaveChangesAsync();
            tx.Commit();

            response.IsSuccess = true;
            response.Data = true;
            response.Message = GlobalMessages.MESSAGE_TRANSACTION;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
