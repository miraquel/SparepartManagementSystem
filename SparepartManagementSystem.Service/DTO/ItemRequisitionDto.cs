namespace SparepartManagementSystem.Service.DTO;

public class ItemRequisitionDto
{
    public int ItemRequisitionId { get; init; }
    public int WorkOrderLineId { get; init; }
    public string ItemId { get; init; } = string.Empty;
    public string ItemName { get; init; } = string.Empty;
    public DateTime RequiredDate { get; init; }
    public decimal Quantity { get; init; }
    public decimal RequestQuantity { get; init; }
    public string InventLocationId { get; init; } = string.Empty;
    public string WMSLocationId { get; init; } = string.Empty;
    public string JournalId { get; init; } = string.Empty;
    public bool IsSubmitted { get; init; } 
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; }
}