namespace SparepartManagementSystem.Service.DTO;

public class ItemRequisitionDto
{
    public int ItemRequisitionId { get; init; }
    public int WorkOrderLineId { get; init; }
    public string ItemId { get; init; } = "";
    public string ItemName { get; init; } = "";
    public DateTime RequiredDate { get; init; }
    public decimal Quantity { get; init; }
    public decimal RequestQuantity { get; init; }
    public string InventLocationId { get; init; } = "";
    public string WMSLocationId { get; init; } = "";
    public string JournalId { get; init; } = "";
    public bool? IsSubmitted { get; init; } 
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; }
}