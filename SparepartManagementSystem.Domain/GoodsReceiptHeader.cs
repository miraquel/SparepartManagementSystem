using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class GoodsReceiptHeader : BaseModel
{
    public int GoodsReceiptHeaderId { get; set; }
    public string PackingSlipId { get; set; } = "";
    public DateTime TransDate { get; init; } = SqlDateTime.MinValue.Value;
    public string Description { get; init; } = "";
    public string PurchId { get; set; } = "";
    public string PurchName { get; set; } = "";
    public string OrderAccount { get; set; } = "";
    public string InvoiceAccount { get; set; } = "";
    public string PurchStatus { get; set; } = "";
    public bool? IsSubmitted { get; set; }
    public DateTime SubmittedDate { get; set; } = SqlDateTime.MinValue.Value;   
    public string SubmittedBy { get; set; } = "";

    // ReSharper disable once CollectionNeverQueried.Global
    public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; init; } = new List<GoodsReceiptLine>();
    
    public static GoodsReceiptHeader ForUpdate(GoodsReceiptHeader oldRecord, GoodsReceiptHeader newRecord)
    {
        return new GoodsReceiptHeader
        {
            GoodsReceiptHeaderId = oldRecord.GoodsReceiptHeaderId,
            PackingSlipId = newRecord.PackingSlipId != oldRecord.PackingSlipId ? newRecord.PackingSlipId : "",
            PurchId = newRecord.PurchId != oldRecord.PurchId ? newRecord.PurchId : "",
            PurchName = newRecord.PurchName != oldRecord.PurchName ? newRecord.PurchName : "",
            OrderAccount = newRecord.OrderAccount != oldRecord.OrderAccount ? newRecord.OrderAccount : "",
            InvoiceAccount = newRecord.InvoiceAccount != oldRecord.InvoiceAccount ? newRecord.InvoiceAccount : "",
            PurchStatus = newRecord.PurchStatus != oldRecord.PurchStatus ? newRecord.PurchStatus : "",
            IsSubmitted = newRecord.IsSubmitted != null && newRecord.IsSubmitted != oldRecord.IsSubmitted ? newRecord.IsSubmitted : oldRecord.IsSubmitted,
            SubmittedBy = newRecord.SubmittedBy != oldRecord.SubmittedBy ? newRecord.SubmittedBy : "",
            SubmittedDate = newRecord.SubmittedDate != SqlDateTime.MinValue.Value && newRecord.SubmittedDate != oldRecord.SubmittedDate ? newRecord.SubmittedDate : SqlDateTime.MinValue.Value,
            CreatedBy = newRecord.CreatedBy != oldRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = newRecord.CreatedDateTime != SqlDateTime.MinValue.Value && newRecord.CreatedDateTime != oldRecord.CreatedDateTime ? newRecord.CreatedDateTime : SqlDateTime.MinValue.Value,
            ModifiedBy = newRecord.ModifiedBy != oldRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = newRecord.ModifiedDateTime != SqlDateTime.MinValue.Value && newRecord.ModifiedDateTime != oldRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : SqlDateTime.MinValue.Value
        };
    }

    public GoodsReceiptHeader Clone()
    {
        return new GoodsReceiptHeader
        {
            GoodsReceiptHeaderId = GoodsReceiptHeaderId,
            PackingSlipId = PackingSlipId,
            TransDate = TransDate,
            Description = Description,
            PurchId = PurchId,
            PurchName = PurchName,
            OrderAccount = OrderAccount,
            InvoiceAccount = InvoiceAccount,
            PurchStatus = PurchStatus,
            IsSubmitted = IsSubmitted,
            SubmittedDate = SubmittedDate,
            SubmittedBy = SubmittedBy,
            CreatedBy = CreatedBy,
            CreatedDateTime = CreatedDateTime,
            ModifiedBy = ModifiedBy,
            ModifiedDateTime = ModifiedDateTime
        };
    }
}