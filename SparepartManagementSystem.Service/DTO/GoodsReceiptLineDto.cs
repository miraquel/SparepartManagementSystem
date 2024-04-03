using System.Data.SqlTypes;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptLineDto
{
    public GoodsReceiptLineDto ForUpdate(GoodsReceiptLineDto dto)
    {
        return new GoodsReceiptLineDto
        {
            GoodsReceiptLineId = GoodsReceiptLineId,
            GoodsReceiptHeaderId = dto.GoodsReceiptHeaderId > 0 ? dto.GoodsReceiptHeaderId : GoodsReceiptHeaderId,
            ItemId = dto.ItemId != ItemId ? dto.ItemId : ItemId,
            LineNumber = dto.LineNumber != LineNumber ? dto.LineNumber : LineNumber,
            ItemName = dto.ItemName != ItemName ? dto.ItemName : ItemName,
            RemainPurchPhysical = dto.RemainPurchPhysical != RemainPurchPhysical ? dto.RemainPurchPhysical : RemainPurchPhysical,
            ReceiveNow = dto.ReceiveNow != ReceiveNow ? dto.ReceiveNow : ReceiveNow,
            PurchQty = dto.PurchQty != PurchQty ? dto.PurchQty : PurchQty,
            PurchUnit = dto.PurchUnit != PurchUnit ? dto.PurchUnit : PurchUnit,
            PurchPrice = dto.PurchPrice != PurchPrice ? dto.PurchPrice : PurchPrice,
            LineAmount = dto.LineAmount != LineAmount ? dto.LineAmount : LineAmount,
            InventLocationId = dto.InventLocationId != InventLocationId ? dto.InventLocationId : InventLocationId,
            WMSLocationId = dto.WMSLocationId != WMSLocationId ? dto.WMSLocationId : WMSLocationId,
            CreatedBy = dto.CreatedBy != CreatedBy ? dto.CreatedBy : CreatedBy,
            CreatedDateTime = dto.CreatedDateTime != SqlDateTime.MinValue.Value ? dto.CreatedDateTime : CreatedDateTime,
            ModifiedBy = dto.ModifiedBy != ModifiedBy ? dto.ModifiedBy : ModifiedBy,
            ModifiedDateTime = dto.ModifiedDateTime != SqlDateTime.MinValue.Value ? dto.ModifiedDateTime : ModifiedDateTime
        };
    }
    
    public int GoodsReceiptLineId { get; init; }
    public int GoodsReceiptHeaderId { get; init; }
    public string ItemId { get; init; } = "";
    public int LineNumber { get; init; }
    public string ItemName { get; init; } = "";
    public ProductType ProductType { get; init; }
    public decimal RemainPurchPhysical { get; init; }
    public decimal ReceiveNow { get; init; }
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