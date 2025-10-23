using Identity.Application.Dtos.Menus;
using Identity.Application.Interfaces.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;
using Helper = SharedKernel.Helpers.Helpers;

namespace Identity.Application.UseCases.Menus.Queries.GetAllQuery
{
    public class GetAllMenuHandler(IUnitOfWork unitOfWork, IOrderingQuery orderingQuery)
        : IQueryHandler<GetAllMenuQuery, IEnumerable<MenuCrudResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IOrderingQuery _orderingQuery = orderingQuery;

        public async Task<BaseResponse<IEnumerable<MenuCrudResponseDto>>> Handle(GetAllMenuQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<MenuCrudResponseDto>>();

            try
            {
                var query = _unitOfWork.Menu.GetAllQueryable();

                // NumFilter: 1=Name, 2=Url
                if (request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
                {
                    switch (request.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.Name.Contains(request.TextFilter));
                            break;
                        case 2:
                            query = query.Where(x => x.Url!.Contains(request.TextFilter));
                            break;
                    }
                }

                if (request.StateFilter is not null)
                {
                    var stateFilter = Helper.SplitStateFilter(request.StateFilter);
                    query = query.Where(x => stateFilter.Contains(x.State));
                }

                if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                {
                    query = query.Where(x => x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                                             x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1));
                }

                request.Sort ??= "Id";

                var items = await _orderingQuery.Ordering(request, query).ToListAsync(cancellationToken);

                response.IsSuccess = true;
                response.TotalRecords = await query.CountAsync(cancellationToken);
                response.Data = items.Adapt<IEnumerable<MenuCrudResponseDto>>();
                response.Message = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
