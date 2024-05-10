using Newtonsoft.Json;

namespace SparepartManagementSystem.Domain;

public class Role : BaseModel
{
    public static Role ForUpdate(Role oldRecord, Role newRecord)
    {
        return new Role
        {
            RoleId = oldRecord.RoleId,
            RoleName = oldRecord.RoleName != newRecord.RoleName ? newRecord.RoleName : "",
            Description = oldRecord.Description != newRecord.Description ? newRecord.Description : "",
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : DateTime.MinValue,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : DateTime.MinValue
        };
    }
    public string Description { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<User> Users { get; set; } = new();
}