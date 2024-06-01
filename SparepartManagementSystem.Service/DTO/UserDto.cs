using System.ComponentModel;
using System.Data.SqlTypes;
using Newtonsoft.Json;

namespace SparepartManagementSystem.Service.DTO;

public class UserDto
{
    [DefaultValue("")]
    public string CreatedBy { get; init; } = string.Empty;

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string Email { get; init; } = string.Empty;

    [DefaultValue("")]
    public string FirstName { get; init; } = string.Empty;
    
    [DefaultValue("")]
    public string LastName { get; init; } = string.Empty;

    [DefaultValue("")]
    public string ModifiedBy { get; init; } = string.Empty;

    [DefaultValue(typeof(DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;

    public ICollection<RoleDto> Roles { get; init; } = [];
    public ICollection<UserWarehouseDto> UserWarehouses { get; init; } = [];

    public int UserId { get; init; }

    [DefaultValue("")]
    public string Username { get; init; } = string.Empty;
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool IsAdministrator { get; init; }
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
    public bool IsEnabled { get; init; }
}