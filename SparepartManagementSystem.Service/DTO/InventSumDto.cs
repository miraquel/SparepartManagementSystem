namespace SparepartManagementSystem.Service.DTO;

public class InventSumDto
{
    public string ItemId { get; init; } = string.Empty;
    public string ItemName { get; init; } = string.Empty;
    public string InventLocationId { get; init; } = string.Empty;
    public string WMSLocationId { get; init; } = string.Empty;
    public decimal PhysicalInvent { get; init; }
    public decimal ReservPhysical { get; init; }
    public decimal AvailPhysical { get; init; }
    public decimal OrderedSum { get; init; }
    public decimal OnOrder { get; init; }
    public decimal ReservOrdered { get; init; }
    public decimal AvailOrdered { get; init; }
}