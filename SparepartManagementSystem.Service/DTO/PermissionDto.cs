using System.ComponentModel;

namespace SparepartManagementSystem.Service.DTO;

public class PermissionDto
{
    public int PermissionId { get; init; }
    [DefaultValue("")]
    public string Module { get; init; } = string.Empty;
    [DefaultValue("")]
    public string Type { get; init; } = string.Empty;
    [DefaultValue("")]
    public string PermissionName { get; init; } = string.Empty;
    public int RoleId { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; } = DateTime.MinValue;
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; } = DateTime.MinValue;
}