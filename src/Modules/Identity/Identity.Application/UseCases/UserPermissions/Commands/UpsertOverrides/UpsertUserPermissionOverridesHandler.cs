using Identity.Application.Dtos.UserPermissions;
using Identity.Application.Interfaces.Services;
using Identity.Application.Interfaces.RealTime; // 👈
using Identity.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.UserPermissions.Commands.UpsertOverrides;

public sealed class UpsertUserPermissionOverridesHandler(IUnitOfWork uow, IPermissionsNotifier notifier)
    : ICommandHandler<UpsertUserPermissionOverridesCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IPermissionsNotifier _notifier = notifier;

    public async Task<BaseResponse<bool>> Handle(UpsertUserPermissionOverridesCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var userId = request.Request.UserId;

            var now = DateTime.Now;
            var currentUserId = 1;

            var entities = request.Request.Overrides.Select(o => new UserPermission
            {
                UserId = userId,
                PermissionId = o.PermissionId,
                IsGranted = o.IsGranted,
                State = "1",
                AuditCreateUser = currentUserId,
                AuditCreateDate = now
            });

            var ok = await _uow.UserPermission.ReplaceOverridesAsync(userId, entities);

            if (ok)
            {
                // 🔔 Notificar sin acoplar a SignalR ni a la capa Api
                await _notifier.NotifyUserPermissionsChangedAsync(userId, cancellationToken);
            }

            response.IsSuccess = ok;
            response.Data = ok;
            response.Message = ok ? GlobalMessages.MESSAGE_TRANSACTION : "No fue posible registrar los permisos del usuario.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
