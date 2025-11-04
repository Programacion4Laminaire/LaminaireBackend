using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure.Authentication;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class HasPermissionAttribute(string permissionSlug)
    : AuthorizeAttribute($"{PermissionAuthorizationPolicyProvider.PolicyPrefix}{permissionSlug}");
