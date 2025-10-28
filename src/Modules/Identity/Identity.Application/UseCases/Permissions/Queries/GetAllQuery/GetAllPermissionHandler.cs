using Identity.Application.Dtos.Permissions;
using Identity.Application.Interfaces.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Identity.Application.UseCases.Permissions.Queries.GetAllQuery;

public class GetAllPermissionHandler(IUnitOfWork uow, IOrderingQuery orderingQuery)
    : IQueryHandler<GetAllPermissionQuery, IEnumerable<PermissionCrudResponseDto>>
{
    public async Task<BaseResponse<IEnumerable<PermissionCrudResponseDto>>> Handle(GetAllPermissionQuery request, CancellationToken ct)
    {
        var response = new BaseResponse<IEnumerable<PermissionCrudResponseDto>>();

        try
        {
            var query = uow.Permission.GetAllQueryable();

            if (request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
            {
                switch (request.NumFilter)
                {
                    case 1: // por nombre
                        query = query.Where(x => x.Name.Contains(request.TextFilter));
                        break;
                    case 2: // por slug
                        query = query.Where(x => x.Slug.Contains(request.TextFilter));
                        break;
                }
            }

            if (request.StateFilter is not null)
            {
                var states = Helper.SplitStateFilter(request.StateFilter);
                query = query.Where(x => states.Contains(x.State));
            }

            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                var start = Convert.ToDateTime(request.StartDate);
                var end = Convert.ToDateTime(request.EndDate).AddDays(1);
                query = query.Where(x => x.AuditCreateDate >= start && x.AuditCreateDate <= end);
            }

            request.Sort ??= "Id";
            var items = await orderingQuery.Ordering(request, query).ToListAsync(ct);

            response.IsSuccess = true;
            response.TotalRecords = await query.CountAsync(ct);
            response.Data = items.Adapt<IEnumerable<PermissionCrudResponseDto>>();
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
