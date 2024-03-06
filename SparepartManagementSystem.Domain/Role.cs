using System.Data.SqlTypes;
using Newtonsoft.Json;

namespace SparepartManagementSystem.Domain;

public class Role : BaseModel
{
    public string Description { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<User> Users { get; set; } = new();
}