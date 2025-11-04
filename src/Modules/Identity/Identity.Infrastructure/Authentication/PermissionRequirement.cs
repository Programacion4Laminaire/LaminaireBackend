using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure.Authentication;

public sealed class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
