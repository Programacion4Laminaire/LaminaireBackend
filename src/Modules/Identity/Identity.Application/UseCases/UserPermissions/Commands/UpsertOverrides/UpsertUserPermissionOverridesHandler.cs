using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.UserPermissions.Commands.UpsertOverrides
{
    public sealed class UpsertUserPermissionOverridesHandler : ICommandHandler<UpsertUserPermissionOverridesCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public UpsertUserPermissionOverridesHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BaseResponse<bool>> Handle(
            UpsertUserPermissionOverridesCommand request,
            CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var userId = request.Request.UserId;

                // Construye los overrides EXACTAMENTE como vienen de la UI (isGranted = estado del check).
                var now = DateTime.Now;
                var currentUserId = 1; // TODO: reemplazar por el usuario autenticado si lo tienes disponible

                var entities = request.Request.Overrides.Select(o => new UserPermission
                {
                    UserId = userId,
                    PermissionId = o.PermissionId,
                    IsGranted = o.IsGranted,   // <- true/false según lo marcado
                    State = "1",
                    AuditCreateUser = currentUserId,
                    AuditCreateDate = now
                });

                var ok = await _uow.UserPermission.ReplaceOverridesAsync(userId, entities);

                response.IsSuccess = ok;
                response.Data = ok;
                response.Message = ok
                    ? GlobalMessages.MESSAGE_TRANSACTION
                    : "No fue posible registrar los permisos del usuario.";
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
