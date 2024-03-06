using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptHeaderDto
{
    public int GoodsReceiptHeaderId { get; init; }

    [DefaultValue("")]
    public string PackingSlipId { get; init; } = string.Empty;

    [DefaultValue("")]
    public string PurchId { get; init; } = string.Empty;

    [DefaultValue("")]
    public string PurchName { get; init; } = string.Empty;

    [DefaultValue("")]
    public string OrderAccount { get; init; } = string.Empty;

    [DefaultValue("")]
    public string InvoiceAccount { get; init; } = string.Empty;

    [DefaultValue("")]
    public string PurchStatus { get; init; } = string.Empty;

    [DefaultValue("")]
    public string SubmittedBy { get; init; } = string.Empty;

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime SubmittedDate { get; init; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string CreatedBy { get; set; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string ModifiedBy { get; set; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
}