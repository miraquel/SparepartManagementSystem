using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptLineDto
{
    public int GoodsReceiptLineId { get; init; }
    public int GoodsReceiptHeaderId { get; init; }
    public string ItemId { get; init; } = string.Empty;
    public int LineNumber { get; init; }
    public string ItemName { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public decimal RemainPurchPhysical { get; init; }
    public decimal ReceiveNow { get; init; }
    public decimal PurchQty { get; init; }
    public string PurchUnit { get; init; } = string.Empty;
    public decimal PurchPrice { get; init; }
    public decimal LineAmount { get; init; }
    public string InventLocationId { get; init; } = string.Empty;
    public string WMSLocationId { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
}