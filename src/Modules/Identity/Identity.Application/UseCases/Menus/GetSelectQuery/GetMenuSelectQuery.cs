using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.UseCases.Menus.Queries.GetSelectQuery
{
    public class GetMenuSelectQuery : IQuery<IEnumerable<SelectResponseDto>> { }
}
