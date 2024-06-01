using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class InventReqDto
{
    public string ItemId { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public DateTime RequiredDate { get; init; } = SqlDateTime.MinValue.Value;
    public decimal Qty { get; init; }
    public long UnitOfMeasure { get; init; }
    public string Currency { get; init; } = string.Empty;
    public decimal CostPrice { get; init; }
    public decimal CostAmount { get; init; }
    public string InventSiteId { get; init; } = string.Empty;
    public string InventLocationId { get; init; } = string.Empty;
    public string WMSLocationId { get; init; } = string.Empty;
    public long AGSWORecId { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; }
    public long RecId { get; init; }
}