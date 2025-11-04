
namespace Identity.Domain.Entities;

public class UserPermission : BaseEntity
{
    public int UserId { get; init; }
    public int PermissionId { get; init; }

    /// <summary>
    /// true = grant (agrega); false = deny (revoca).
    /// </summary>
    public bool IsGranted { get; init; }

    public User User { get; init; } = null!; 
    public Permission Permission { get; init; } = null!;
}
