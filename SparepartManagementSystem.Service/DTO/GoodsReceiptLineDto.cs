using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptLineDto
{
    public int GoodsReceiptLineId { get; init; }
    public int GoodsReceiptHeaderId { get; init; }
    public string ItemId { get; init; } = "";
    public int LineNumber { get; init; }
    public string ItemName { get; init; } = "";
    public decimal PurchQty { get; init; }
    public string PurchUnit { get; init; } = "";
    public decimal PurchPrice { get; init; }
    public decimal LineAmount { get; init; }
    public string InventLocationId { get; init; } = "";
    public string WMSLocationId { get; init; } = "";
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
}