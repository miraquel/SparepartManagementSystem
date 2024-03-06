using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptLineDto
{
    public int GoodsReceiptLineId { get; init; }
    public int GoodsReceiptHeaderId { get; init; }
    public string ItemId { get; init; } = "";
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal Amount { get; init; }
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
}