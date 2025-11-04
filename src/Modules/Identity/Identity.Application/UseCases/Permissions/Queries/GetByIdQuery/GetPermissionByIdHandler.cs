using Identity.Application.Dtos.Permissions;
using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Permissions.Queries.GetByIdQuery;

public class GetPermissionByIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetPermissionByIdQuery, PermissionCrudByIdResponseDto>
{
    public async Task<BaseResponse<PermissionCrudByIdResponseDto>> Handle(GetPermissionByIdQuery request, CancellationToken ct)
    {
        var response = new BaseResponse<PermissionCrudByIdResponseDto>();

        try
        {
            var entity = await uow.Permission.GetByIdAsync(request.PermissionId);
            if (entity is null)
            {
                response.IsSuccess = false;
                response.Message = "El permiso no existe.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = entity.Adapt<PermissionCrudByIdResponseDto>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
