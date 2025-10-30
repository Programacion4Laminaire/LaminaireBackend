using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Identity.Infrastructure.Authentication;

public sealed class PermissionAuthorizationHandler(IPermissionService permissionService)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService = permissionService;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // UserId desde claim (ajusta si usas otro).
        var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(idClaim) || !int.TryParse(idClaim, out var userId))
            return;

        var slugs = await _permissionService.GetEffectivePermissionSlugsAsync(userId);
        if (slugs.Contains(requirement.Permission))
            context.Succeed(requirement);
    }
}
