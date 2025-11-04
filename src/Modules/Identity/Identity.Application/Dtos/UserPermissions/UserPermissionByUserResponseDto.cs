
namespace Identity.Application.Dtos.UserPermissions;

public sealed record UserPermissionByUserResponseDto
{
    public int PermissionId { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Slug { get; init; } = null!;
    public int MenuId { get; init; }
    public string MenuName { get; init; } = null!;

    /// <summary>Si el usuario, luego de aplicar rol + overrides, posee el permiso.</summary>
    public bool Effective { get; init; }

    /// <summary>Si existe override explícito para este permiso y si es grant/deny.</summary>
    public bool? OverrideIsGranted { get; init; } // null=no override; true=grant; false=deny
}
