namespace SparepartManagementSystem.Service.DTO;

public class InventSumSearchDto
{
    public string ItemId { get; init; } = string.Empty;
    public string InventLocationId { get; init; } = string.Empty;
    public string WMSLocationId { get; init; } = string.Empty;
}