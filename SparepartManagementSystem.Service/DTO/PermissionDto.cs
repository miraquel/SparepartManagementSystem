using System.ComponentModel;

namespace SparepartManagementSystem.Service.DTO;

public class PermissionDto
{
    public int PermissionId { get; set; }
    [DefaultValue("")]
    public string Module { get; set; } = "";
    [DefaultValue("")]
    public string Type { get; set; } = "";
    [DefaultValue("")]
    public string PermissionName { get; init; } = "";
    public int RoleId { get; set; }
}