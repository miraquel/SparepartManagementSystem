namespace SparepartManagementSystem.Service.DTO;

public class UserWarehouseDto
{
    public int UserWarehouseId { get; init; }
    public int UserId { get; init; }
    public string InventLocationId { get; init; } = string.Empty;
    public string InventSiteId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public bool IsDefault { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; }
}