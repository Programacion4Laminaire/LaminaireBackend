using Identity.Application.Dtos.Menus;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Dtos.Commons;

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

        config.NewConfig<Menu, MenuCrudResponseDto>()
                .Map(d => d.MenuId, s => s.Id)
                .Map(d => d.StateDescription, s => s.State == "1" ? "Activo" : "Inactivo")
                .TwoWays();

        config.NewConfig<Menu, MenuCrudByIdResponseDto>()
                        .Map(d => d.MenuId, s => s.Id)
                        .TwoWays();

        config.NewConfig<Menu, SelectResponseDto>()
                        .Map(d => d.Code, s => s.Id)
                        .Map(d => d.Description, s => s.Name)
                        .TwoWays();

    }
}



