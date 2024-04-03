namespace SparepartManagementSystem.Domain;

public class WorkOrderLine : BaseModel
{
    public int WorkOrderLineId { get; set; }
    public int WorkOrderHeaderId { get; set; }
    public string ItemId { get; set; } = "";
    public string ItemName { get; set; } = "";
    public DateTime RequiredDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal RequestQuantity { get; set; }
    public string InventLocationId { get; set; } = "";
    public string WMSLocationId { get; set; } = "";
}