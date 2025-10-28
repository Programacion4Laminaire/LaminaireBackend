
using Identity.Application.Dtos.MenuRoles;
using Identity.Domain.Entities;
using Mapster;

namespace Identity.Application.Mappings;

public class MenuRoleMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Menu, MenuRoleByRoleResponseDto>()
              .Map(d => d.MenuId, s => s.Id)
              .Map(d => d.FatherId, s => s.FatherId)
              .Map(d => d.Name, s => s.Name)
              .Map(d => d.Icon, s => s.Icon)
              .Map(d => d.Url, s => s.Url);
    }
}
