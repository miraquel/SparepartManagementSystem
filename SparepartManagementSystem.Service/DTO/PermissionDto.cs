using System.ComponentModel;

namespace SparepartManagementSystem.Service.DTO;

public class PermissionDto
{
    public int PermissionId { get; init; }
    [DefaultValue("")]
    public string Module { get; init; } = "";
    [DefaultValue("")]
    public string Type { get; init; } = "";
    [DefaultValue("")]
    public string PermissionName { get; init; } = "";
    public int RoleId { get; init; }
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedDateTime { get; set; } = DateTime.MinValue;
    public string ModifiedBy { get; set; } = "";
    public DateTime ModifiedDateTime { get; set; } = DateTime.MinValue;
}