using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class GoodsReceiptHeaderDto
{
    public GoodsReceiptHeaderDto Merge(GoodsReceiptHeaderDto dto)
    {
        return new GoodsReceiptHeaderDto
        {
            GoodsReceiptHeaderId = dto.GoodsReceiptHeaderId > 0 ? dto.GoodsReceiptHeaderId : GoodsReceiptHeaderId,
            PackingSlipId = !string.IsNullOrEmpty(dto.PackingSlipId) ? dto.PackingSlipId : PackingSlipId,
            PurchId = !string.IsNullOrEmpty(dto.PurchId) ? dto.PurchId : PurchId,
            PurchName = !string.IsNullOrEmpty(dto.PurchName) ? dto.PurchName : PurchName,
            OrderAccount = !string.IsNullOrEmpty(dto.OrderAccount) ? dto.OrderAccount : OrderAccount,
            InvoiceAccount = !string.IsNullOrEmpty(dto.InvoiceAccount) ? dto.InvoiceAccount : InvoiceAccount,
            PurchStatus = !string.IsNullOrEmpty(dto.PurchStatus) ? dto.PurchStatus : PurchStatus,
            IsSubmitted = dto.IsSubmitted ?? IsSubmitted,
            SubmittedBy = !string.IsNullOrEmpty(dto.SubmittedBy) ? dto.SubmittedBy : SubmittedBy,
            SubmittedDate = dto.SubmittedDate != SqlDateTime.MinValue.Value ? dto.SubmittedDate : SubmittedDate,
            GoodsReceiptLines = dto.GoodsReceiptLines.Count > 0 ? dto.GoodsReceiptLines : GoodsReceiptLines,
            CreatedBy = !string.IsNullOrEmpty(dto.CreatedBy) ? dto.CreatedBy : CreatedBy,
            CreatedDateTime = dto.CreatedDateTime != SqlDateTime.MinValue.Value ? dto.CreatedDateTime : CreatedDateTime,
            ModifiedBy = !string.IsNullOrEmpty(dto.ModifiedBy) ? dto.ModifiedBy : ModifiedBy,
            ModifiedDateTime = dto.ModifiedDateTime != SqlDateTime.MinValue.Value ? dto.ModifiedDateTime : ModifiedDateTime
        };
    }
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
    
    public bool? IsSubmitted { get; init; }

    [DefaultValue("")]
    public string SubmittedBy { get; init; } = string.Empty;

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime SubmittedDate { get; init; } = SqlDateTime.MinValue.Value;
    
    public ICollection<GoodsReceiptLineDto> GoodsReceiptLines { get; init; } = new List<GoodsReceiptLineDto>();

    [DefaultValue("")]
    public string CreatedBy { get; set; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;

    [DefaultValue("")]
    public string ModifiedBy { get; set; } = "";

    [DefaultValue(typeof (DateTime), "1753-01-01T00:00:00")]
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
}