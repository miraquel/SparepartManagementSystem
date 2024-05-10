namespace SparepartManagementSystem.Domain;

public class ItemRequisition : BaseModel
{
    public static ItemRequisition ForUpdate(ItemRequisition oldRecord, ItemRequisition newRecord)
    {
        return new ItemRequisition
        {
            ItemRequisitionId = oldRecord.ItemRequisitionId,
            WorkOrderLineId = oldRecord.WorkOrderLineId != newRecord.WorkOrderLineId ? newRecord.WorkOrderLineId : 0,
            ItemId = oldRecord.ItemId != newRecord.ItemId ? newRecord.ItemId : "",
            ItemName = oldRecord.ItemName != newRecord.ItemName ? newRecord.ItemName : "",
            RequiredDate = oldRecord.RequiredDate != newRecord.RequiredDate ? newRecord.RequiredDate : DateTime.MinValue,
            Quantity = oldRecord.Quantity != newRecord.Quantity ? newRecord.Quantity : 0,
            RequestQuantity = oldRecord.RequestQuantity != newRecord.RequestQuantity ? newRecord.RequestQuantity : 0,
            InventLocationId = oldRecord.InventLocationId != newRecord.InventLocationId ? newRecord.InventLocationId : "",
            WMSLocationId = oldRecord.WMSLocationId != newRecord.WMSLocationId ? newRecord.WMSLocationId : "",
            JournalId = oldRecord.JournalId != newRecord.JournalId ? newRecord.JournalId : "",
            IsSubmitted = oldRecord.IsSubmitted != newRecord.IsSubmitted ? newRecord.IsSubmitted : null,
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : DateTime.MinValue,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : DateTime.MinValue
        };
    }
    public int ItemRequisitionId { get; set; }
    public int WorkOrderLineId { get; set; }
    public string ItemId { get; set; } = "";
    public string ItemName { get; set; } = "";
    public DateTime RequiredDate { get; set; }
    public decimal Quantity { get; set; }
    public decimal RequestQuantity { get; set; }
    public string InventLocationId { get; set; } = "";
    public string WMSLocationId { get; set; } = "";
    public string JournalId { get; set; } = "";
    public bool? IsSubmitted { get; set; }
}