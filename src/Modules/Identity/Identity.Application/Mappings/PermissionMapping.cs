using Identity.Application.Dtos.Permissions;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.Mappings;

public class PermissionMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Para Role-Permissions (ya existía)
        config.NewConfig<Permission, PermissionsResponseDto>()
          .Map(d => d.PermissionId, s => s.Id)
          .Map(d => d.PermissionName, s => s.Name)
          .Map(d => d.PermissionDescription, s => s.Description)
          .TwoWays();

        // CRUD list
        config.NewConfig<Permission, PermissionCrudResponseDto>()
          .Map(d => d.PermissionId, s => s.Id)
          .Map(d => d.StateDescription, s => s.State == "1" ? "Activo" : "Inactivo")
          .TwoWays();

        // CRUD by id
        config.NewConfig<Permission, PermissionCrudByIdResponseDto>()
          .Map(d => d.PermissionId, s => s.Id)
          .TwoWays();

        // Select común
        config.NewConfig<Permission, SelectResponseDto>()
          .Map(d => d.Code, s => s.Id)
          .Map(d => d.Description, s => s.Name)
          .TwoWays();
    }
}
