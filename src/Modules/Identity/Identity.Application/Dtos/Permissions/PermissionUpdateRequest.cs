using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Dtos.Permissions;

public class PermissionUpdateRequest
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Slug { get; set; } = null!;
    public int MenuId { get; set; }
    public string State { get; set; } = "1"; // en backend el campo es string, no int
}