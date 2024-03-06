using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class RoleDto
{
    [DefaultValue("")] public string CreatedBy { get; set; } = "";
    
    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")] 
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    
    [DefaultValue("")] public string Description { get; set; } = "";
    
    [DefaultValue("")] public string ModifiedBy { get; set; } = "";

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    
    public int RoleId { get; set; }
    
    [DefaultValue("")] public string RoleName { get; set; } = "";
    public List<UserDto>? Users { get; init; }
}