using Identity.Application.Dtos.Menus;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.Mappings;

public class MenuMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        
        config.NewConfig<Menu, MenuResponseDto>()
          .Map(d => d.MenuId, s => s.Id)
          .Map(d => d.Item, s => s.Name)
          .Map(d => d.Icon, s => s.Icon)
          .Map(d => d.Path, s => s.Url)
          .Map(d => d.FatherId, s => s.FatherId)
          .Map(d => d.IsNew, s => s.IsNew);

       
    }
}
