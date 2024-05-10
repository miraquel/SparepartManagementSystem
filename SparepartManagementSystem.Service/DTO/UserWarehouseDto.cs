namespace SparepartManagementSystem.Service.DTO;

public class UserWarehouseDto
{
    public int UserWarehouseId { get; init; }
    public int UserId { get; init; }
    public string InventLocationId { get; init; } = "";
    public string Name { get; init; } = "";
    public bool IsDefault { get; init; }
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; }
}