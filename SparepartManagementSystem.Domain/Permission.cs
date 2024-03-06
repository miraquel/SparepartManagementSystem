using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class Permission : BaseModel
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}