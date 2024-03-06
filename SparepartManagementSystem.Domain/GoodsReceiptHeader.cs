// final int goodsReceiptHeaderId;
// final String packingSlipId;
// final String purchId;
// final String purchName;
// final String orderAccount;
// final String invoiceAccount;
// final String purchStatus;
// final DateTime submittedDate;
// final String submittedBy;

using System.Data.SqlTypes;
using SparepartManagementSystem.Domain;

public class GoodsReceiptHeader : BaseModel
{
    public int GoodsReceiptHeaderId { get; set; }
    public string PackingSlipId { get; set; } = "";
    public string PurchId { get; set; } = "";
    public string PurchName { get; set; } = "";
    public string OrderAccount { get; set; } = "";
    public string InvoiceAccount { get; set; } = "";
    public string PurchStatus { get; set; } = "";
    public DateTime SubmittedDate { get; set; } = SqlDateTime.MinValue.Value;
    public string SubmittedBy { get; set; } = "";
}