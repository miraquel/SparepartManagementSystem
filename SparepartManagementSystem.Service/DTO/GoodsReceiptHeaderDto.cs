using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptHeaderDto
{
    public int GoodsReceiptHeaderId { get; init; }

    [DefaultValue("")]
    public string PackingSlipId { get; init; } = string.Empty;
    
    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime TransDate { get; init; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string Description { get; init; } = "";

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
    
    public bool? IsSubmitted { get; init; }

    [DefaultValue("")]
    public string SubmittedBy { get; init; } = string.Empty;

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime SubmittedDate { get; init; } = SqlDateTime.MinValue.Value;
    
    public ICollection<GoodsReceiptLineDto> GoodsReceiptLines { get; init; } = new List<GoodsReceiptLineDto>();

    [DefaultValue("")]
    public string CreatedBy { get; init; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string ModifiedBy { get; init; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
}