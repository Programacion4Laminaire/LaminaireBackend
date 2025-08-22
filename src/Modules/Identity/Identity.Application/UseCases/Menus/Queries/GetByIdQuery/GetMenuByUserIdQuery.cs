using Identity.Application.Dtos.Menus;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Menus.Queries.GetByIdQuery;

public class GetMenuByUserIdQuery : IQuery<IEnumerable<MenuResponseDto>>
{
    public int UserId { get; set; }
}
