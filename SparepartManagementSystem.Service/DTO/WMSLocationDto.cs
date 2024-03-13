using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.DTO;

public class WMSLocationDto
{
    public string InventLocationId { get; init; } = "";
    public string WMSLocationId { get; init; } = "";
    public WMSLocationType LocationType { get; init; }
    public int MaxPalletCount { get; init; }
    public decimal MaxWeight { get; init; }
    public decimal MaxVolume { get; init; }
}