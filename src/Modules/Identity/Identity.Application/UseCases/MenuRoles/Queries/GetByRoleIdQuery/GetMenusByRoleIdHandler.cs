
using Identity.Application.Dtos.MenuRoles;
using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.MenuRoles.Queries.GetByRoleIdQuery;

public class GetMenusByRoleIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetMenusByRoleIdQuery, IEnumerable<MenuRoleByRoleResponseDto>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<IEnumerable<MenuRoleByRoleResponseDto>>> Handle(
        GetMenusByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<MenuRoleByRoleResponseDto>>();

        try
        {
            var menus = await _uow.Menu.GetMenuPermissionByRoleIdAsync(0);
            var assigned = await _uow.Menu.GetMenuRolesByRoleId(request.RoleId);
            var selecteds = assigned.Select(a => a.MenuId).ToHashSet();

            var data = menus
                .OrderBy(m => m.Position)
                .Adapt<List<MenuRoleByRoleResponseDto>>()
                .Select(m => m with { Selected = selecteds.Contains(m.MenuId) })
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
