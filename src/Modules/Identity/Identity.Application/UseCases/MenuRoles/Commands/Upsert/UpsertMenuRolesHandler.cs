
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.MenuRoles.Commands.Upsert;

public class UpsertMenuRolesHandler(IUnitOfWork uow) : ICommandHandler<UpsertMenuRolesCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(UpsertMenuRolesCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        using var tx = _uow.BeginTransaction();
        try
        {
            var roleId = request.Request.RoleId;

            // 1) Eliminar actuales
            var current = await _uow.Menu.GetMenuRolesByRoleId(roleId);
            if (current.Count > 0)
            {
                var removed = await _uow.Menu.DeleteMenuRole(current);
                if (!removed) throw new InvalidOperationException("No fue posible eliminar las asignaciones actuales.");
            }

            // 2) Insertar nuevas
            if (request.Request.MenuIds != null && request.Request.MenuIds.Any())
            {
                var toInsert = request.Request.MenuIds.Select(mid => new MenuRole
                {
                    RoleId = roleId,
                    MenuId = mid,
                    State = "1"
                });

                var created = await _uow.Menu.RegisterRoleMenus(toInsert);
                if (!created) throw new InvalidOperationException("No fue posible registrar las nuevas asignaciones.");
            }

            await _uow.SaveChangesAsync();
            tx.Commit();

            response.IsSuccess = true;
            response.Data = true;
            response.Message = GlobalMessages.MESSAGE_TRANSACTION; // "Transacción realizada correctamente."
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
