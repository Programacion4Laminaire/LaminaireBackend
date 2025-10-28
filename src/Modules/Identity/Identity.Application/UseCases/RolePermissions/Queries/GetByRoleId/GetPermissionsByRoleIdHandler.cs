
using Identity.Application.Dtos.RolePermissions;
using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.RolePermissions.Queries.GetByRoleId;

public sealed class GetPermissionsByRoleIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetPermissionsByRoleIdQuery, IEnumerable<PermissionByRoleResponseDto>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<IEnumerable<PermissionByRoleResponseDto>>> Handle(
        GetPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<PermissionByRoleResponseDto>>();

        try
        {
            var all = await _uow.Permission.GetAllWithMenuAsync();
            var assignedIds = (await _uow.Permission.GetPermissionIdsByRoleIdAsync(request.RoleId)).ToHashSet();

            var data = all
                .Select(p => new PermissionByRoleResponseDto
                {
                    PermissionId = p.PermissionId,
                    Name = p.Name,
                    Description = p.Description,
                    Slug = p.Slug,
                    MenuId = p.MenuId,
                    MenuName = p.MenuName,
                    Selected = assignedIds.Contains(p.PermissionId)
                })
                .OrderBy(p => p.MenuName).ThenBy(p => p.Name)
                .ToList();

            response.IsSuccess = true;
            response.Message = GlobalMessages.MESSAGE_QUERY;
            response.Data = data;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
