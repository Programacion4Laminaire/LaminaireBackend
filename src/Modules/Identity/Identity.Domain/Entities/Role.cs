namespace Identity.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
