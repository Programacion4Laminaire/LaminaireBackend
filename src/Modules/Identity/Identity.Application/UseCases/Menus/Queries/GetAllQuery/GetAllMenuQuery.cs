using Identity.Application.Dtos.Menus;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Menus.Queries.GetAllQuery
{
    public class GetAllMenuQuery : BaseFilters, IQuery<IEnumerable<MenuCrudResponseDto>> { }
}
