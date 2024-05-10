using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class GoodsReceiptLine : BaseModel, ICloneable
{
    public static GoodsReceiptLine ForUpdate(GoodsReceiptLine oldRecord, GoodsReceiptLine newRecord)
    {
        return new GoodsReceiptLine
        {
            GoodsReceiptLineId = oldRecord.GoodsReceiptLineId,
            GoodsReceiptHeaderId = newRecord.GoodsReceiptHeaderId != oldRecord.GoodsReceiptHeaderId ? newRecord.GoodsReceiptHeaderId : 0,
            ItemId = newRecord.ItemId != oldRecord.ItemId ? newRecord.ItemId : "",
            LineNumber = newRecord.LineNumber != oldRecord.LineNumber ? newRecord.LineNumber : 0,
            ItemName = newRecord.ItemName != oldRecord.ItemName ? newRecord.ItemName : "",
            ProductType = newRecord.ProductType != oldRecord.ProductType ? newRecord.ProductType : ProductType.None,
            RemainPurchPhysical = newRecord.RemainPurchPhysical != oldRecord.RemainPurchPhysical ? newRecord.RemainPurchPhysical : 0,
            ReceiveNow = newRecord.ReceiveNow != oldRecord.ReceiveNow ? newRecord.ReceiveNow : 0,
            PurchQty = newRecord.PurchQty != oldRecord.PurchQty ? newRecord.PurchQty : 0,
            PurchUnit = newRecord.PurchUnit != oldRecord.PurchUnit ? newRecord.PurchUnit : "",
            PurchPrice = newRecord.PurchPrice != oldRecord.PurchPrice ? newRecord.PurchPrice : 0,
            LineAmount = newRecord.LineAmount != oldRecord.LineAmount ? newRecord.LineAmount : 0,
            InventLocationId = newRecord.InventLocationId != oldRecord.InventLocationId ? newRecord.InventLocationId : "",
            WMSLocationId = newRecord.WMSLocationId != oldRecord.WMSLocationId ? newRecord.WMSLocationId : "",
            CreatedBy = newRecord.CreatedBy != oldRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = newRecord.CreatedDateTime != oldRecord.CreatedDateTime ? newRecord.CreatedDateTime : SqlDateTime.MinValue.Value,
            ModifiedBy = newRecord.ModifiedBy != oldRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = newRecord.ModifiedDateTime != oldRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : SqlDateTime.MinValue.Value
        };
    }
    
    public static bool Compare(GoodsReceiptLine oldRecord, GoodsReceiptLine newRecord)
    {
        return oldRecord.GoodsReceiptHeaderId == newRecord.GoodsReceiptHeaderId &&
               oldRecord.ItemId == newRecord.ItemId &&
               oldRecord.LineNumber == newRecord.LineNumber &&
               oldRecord.ItemName == newRecord.ItemName &&
               oldRecord.ProductType == newRecord.ProductType &&
               oldRecord.RemainPurchPhysical == newRecord.RemainPurchPhysical &&
               oldRecord.ReceiveNow == newRecord.ReceiveNow &&
               oldRecord.PurchQty == newRecord.PurchQty &&
               oldRecord.PurchUnit == newRecord.PurchUnit &&
               oldRecord.PurchPrice == newRecord.PurchPrice &&
               oldRecord.LineAmount == newRecord.LineAmount &&
               oldRecord.InventLocationId == newRecord.InventLocationId &&
               oldRecord.WMSLocationId == newRecord.WMSLocationId;
    }
    
    public int GoodsReceiptLineId { get; set; }
    public int GoodsReceiptHeaderId { get; set; }
    public string ItemId { get; set; } = "";
    public int LineNumber { get; set; }
    public string ItemName { get; set; } = "";
    public ProductType ProductType { get; set; }
    public decimal RemainPurchPhysical { get; set; }
    public decimal ReceiveNow { get; set; }
    public decimal PurchQty { get; set; }
    public string PurchUnit { get; set; } = "";
    public decimal PurchPrice { get; set; }
    public decimal LineAmount { get; set; }
    public string InventLocationId { get; set; } = "";
    public string WMSLocationId { get; set; } = "";
    public object Clone()
    {
        return new GoodsReceiptLine
        {
            GoodsReceiptLineId = GoodsReceiptLineId,
            GoodsReceiptHeaderId = GoodsReceiptHeaderId,
            ItemId = ItemId,
            LineNumber = LineNumber,
            ItemName = ItemName,
            ProductType = ProductType,
            RemainPurchPhysical = RemainPurchPhysical,
            ReceiveNow = ReceiveNow,
            PurchQty = PurchQty,
            PurchUnit = PurchUnit,
            PurchPrice = PurchPrice,
            LineAmount = LineAmount,
            InventLocationId = InventLocationId,
            WMSLocationId = WMSLocationId,
            CreatedBy = CreatedBy,
            CreatedDateTime = CreatedDateTime,
            ModifiedBy = ModifiedBy,
            ModifiedDateTime = ModifiedDateTime
        };
    }
}