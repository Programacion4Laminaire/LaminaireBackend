using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Authentication;

/// Crea dinámicamente políticas "Permission:<slug>"
public sealed class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public const string PolicyPrefix = "Permission:";

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName.Substring(PolicyPrefix.Length);
            var builder = new AuthorizationPolicyBuilder();
            builder.AddRequirements(new PermissionRequirement(permission));
            return await Task.FromResult(builder.Build());
        }

        return await base.GetPolicyAsync(policyName);
    }
}
