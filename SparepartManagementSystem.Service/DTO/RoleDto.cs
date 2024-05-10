using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class RoleDto
{
    [DefaultValue("")] public string CreatedBy { get; init; } = "";
    
    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")] 
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    
    [DefaultValue("")] public string Description { get; init; } = "";
    
    [DefaultValue("")] public string ModifiedBy { get; init; } = "";

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    
    public int RoleId { get; init; }
    
    [DefaultValue("")] public string RoleName { get; init; } = "";
    public List<UserDto>? Users { get; init; }
}