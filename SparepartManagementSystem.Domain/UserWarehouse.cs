namespace SparepartManagementSystem.Domain;

public class UserWarehouse : BaseModel
{
    public static UserWarehouse ForUpdate(UserWarehouse oldRecord, UserWarehouse newRecord)
    {
        return new UserWarehouse
        {
            UserWarehouseId = oldRecord.UserWarehouseId,
            UserId = oldRecord.UserId != newRecord.UserId ? newRecord.UserId : 0,
            InventLocationId = oldRecord.InventLocationId != newRecord.InventLocationId ? newRecord.InventLocationId : "",
            Name = oldRecord.Name != newRecord.Name ? newRecord.Name : "",
            IsDefault = oldRecord.IsDefault != newRecord.IsDefault ? newRecord.IsDefault : null,
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : DateTime.MinValue,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : DateTime.MinValue
        };
    }
    public int UserWarehouseId { get; set; }
    public int UserId { get; set; }
    public string InventLocationId { get; set; } = "";
    public string Name { get; set; } = "";
    public bool? IsDefault { get; set; }
}