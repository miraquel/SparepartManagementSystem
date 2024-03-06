using System.ComponentModel;
using System.Data.SqlTypes;
using Newtonsoft.Json;

namespace SparepartManagementSystem.Service.DTO;

public class UserDto
{
    [DefaultValue("")]
    public string CreatedBy { get; set; } = "";

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string Email { get; set; } = "";

    [DefaultValue("")]
    public string FirstName { get; set; } = "";
    
    [DefaultValue("")]
    public string LastName { get; set; } = "";

    [DefaultValue("")]
    public string ModifiedBy { get; set; } = "";

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;

    public ICollection<RoleDto>? Roles { get; init; }

    public int UserId { get; init; }

    [DefaultValue("")]
    public string Username { get; set; } = "";
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool IsAdministrator { get; set; }
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool IsEnabled { get; set; }
}