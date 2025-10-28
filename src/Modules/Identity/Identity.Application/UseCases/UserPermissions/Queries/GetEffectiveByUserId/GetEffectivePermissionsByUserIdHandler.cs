using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Dtos.UserPermissions;
using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.UserPermissions.Queries.GetEffectiveByUserId
{
    public sealed class GetEffectivePermissionsByUserIdHandler : IQueryHandler<GetEffectivePermissionsByUserIdQuery, IEnumerable<UserPermissionByUserResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetEffectivePermissionsByUserIdHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BaseResponse<IEnumerable<UserPermissionByUserResponseDto>>> Handle(
            GetEffectivePermissionsByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<UserPermissionByUserResponseDto>>();

            try
            {
                // 1) Permisos del rol (baseline)
                var rolePermissionIds = (await _uow.UserPermission
                        .GetRolePermissionIdsByUserIdAsync(request.UserId))
                    .ToHashSet();

                // 2) Overrides del usuario (si existen, mandan sobre el rol)
                var overrides = await _uow.UserPermission.GetOverridesByUserIdAsync(request.UserId);
                // permisoId -> isGranted
                var ov = overrides.ToDictionary(x => x.PermissionId, x => x.IsGranted);

                // 3) Universo de permisos con su menú (para agrupar en UI)
                var all = await _uow.UserPermission.GetAllPermissionsWithMenuAsync();

                // 4) Resolver "effective" y mapear DTO
                var data = all
                    .Select(p =>
                    {
                        // Si hay override, manda el override; si no, lo que diga el rol
                        var hasOverride = ov.TryGetValue(p.PermissionId, out var overrideValue);
                        var effective = hasOverride ? overrideValue : rolePermissionIds.Contains(p.PermissionId);
                        bool? overrideFlag = hasOverride ? overrideValue : (bool?)null;

                        return new UserPermissionByUserResponseDto
                        {
                            PermissionId = p.PermissionId,
                            Name = p.Name,
                            Description = p.Description,
                            Slug = p.Slug,
                            MenuId = p.MenuId,
                            MenuName = p.MenuName,
                            Effective = effective,
                            OverrideIsGranted = overrideFlag
                        };
                    })
                    .OrderBy(x => x.MenuName)
                    .ThenBy(x => x.Name)
                    .ToList();

                response.IsSuccess = true;
                response.Message = GlobalMessages.MESSAGE_QUERY;
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
