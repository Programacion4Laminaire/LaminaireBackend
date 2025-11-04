using Identity.Application.Dtos.Menus;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Menus.Queries.GetByIdQuery
{
    public class GetMenuCrudByIdQuery : IQuery<MenuCrudByIdResponseDto>
    {
        public int MenuId { get; set; }
    }
}
