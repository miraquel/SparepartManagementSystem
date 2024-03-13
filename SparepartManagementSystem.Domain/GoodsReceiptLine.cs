using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class GoodsReceiptLine : BaseModel
{
    public int GoodsReceiptLineId { get; set; }
    public int GoodsReceiptHeaderId { get; set; }
    public string ItemId { get; set; } = "";
    public int LineNumber { get; set; }
    public string ItemName { get; set; } = "";
    public decimal PurchQty { get; set; }
    public string PurchUnit { get; set; } = "";
    public decimal PurchPrice { get; set; }
    public decimal LineAmount { get; set; }
    public string InventLocationId { get; set; } = "";
    public string WMSLocationId { get; set; } = "";
}