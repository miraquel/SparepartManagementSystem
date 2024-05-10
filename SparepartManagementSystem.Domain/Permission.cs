using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class Permission : BaseModel
{
    public static Permission ForUpdate(Permission oldRecord, Permission newRecord)
    {
        return new Permission
        {
            PermissionId = oldRecord.PermissionId,
            PermissionName = oldRecord.PermissionName != newRecord.PermissionName ? newRecord.PermissionName : "",
            RoleId = oldRecord.RoleId != newRecord.RoleId ? newRecord.RoleId : 0,
            Module = oldRecord.Module != newRecord.Module ? newRecord.Module : "",
            Type = oldRecord.Type != newRecord.Type ? newRecord.Type : "",
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : SqlDateTime.MinValue.Value,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : SqlDateTime.MinValue.Value
        };
    }
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string Module { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}