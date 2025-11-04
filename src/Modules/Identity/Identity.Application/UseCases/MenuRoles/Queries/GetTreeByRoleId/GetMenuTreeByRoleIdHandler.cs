
using Identity.Application.Dtos.MenuRoles;
using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.MenuRoles.Queries.GetTreeByRoleId;

public sealed class GetMenuTreeByRoleIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetMenuTreeByRoleIdQuery, IEnumerable<MenuRoleTreeResponseDto>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<IEnumerable<MenuRoleTreeResponseDto>>> Handle(
        GetMenuTreeByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<MenuRoleTreeResponseDto>>();

        try
        {
            // Trae TODOS los menús (incluye padres sin URL). Asegúrate que el repo no filtre por Url != null.
            var menus = await _uow.Menu.GetMenuPermissionByRoleIdAsync(0);

            // Asignaciones del rol
            var assigned = await _uow.Menu.GetMenuRolesByRoleId(request.RoleId);
            var assignedIds = assigned.Select(a => a.MenuId).ToHashSet();

            // Cache de nodos
            var nodes = menus
                .Select(m => new MenuRoleTreeResponseDto
                {
                    MenuId = m.Id,
                    FatherId = m.FatherId,
                    Name = m.Name,
                    Icon = m.Icon,
                    Url = m.Url,
                    Position = m.Position,
                    Selected = assignedIds.Contains(m.Id),
                })
                .ToDictionary(n => n.MenuId, n => n);

            // Jerarquía
            var roots = new List<MenuRoleTreeResponseDto>();
            foreach (var node in nodes.Values)
            {
                if (node.FatherId is null)
                {
                    roots.Add(node);
                }
                else if (nodes.TryGetValue(node.FatherId.Value, out var parent))
                {
                    parent.Children.Add(node); // ✅ se puede (no reasigna)
                }
            }

            // Ordenar sin reasignar la colección (evita romper init-only)
            void SortRec(MenuRoleTreeResponseDto n)
            {
                var ordered = n.Children
                    .OrderBy(c => c.Position)
                    .ThenBy(c => c.Name)
                    .ToList();

                n.Children.Clear();
                n.Children.AddRange(ordered);

                foreach (var c in n.Children)
                    SortRec(c);
            }

            roots = roots
                .OrderBy(r => r.Position)
                .ThenBy(r => r.Name)
                .ToList();

            foreach (var r in roots)
                SortRec(r);

            response.IsSuccess = true;
            response.Message = GlobalMessages.MESSAGE_QUERY;
            response.Data = roots;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
