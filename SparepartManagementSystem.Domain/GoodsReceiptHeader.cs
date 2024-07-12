using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class GoodsReceiptHeader : BaseModel
{
    private int _goodsReceiptHeaderId;
    public int GoodsReceiptHeaderId
    {
        get => _goodsReceiptHeaderId;
        set
        {
            if (_goodsReceiptHeaderId == value)
            {
                return;
            }

            _goodsReceiptHeaderId = value;
            IsChanged = true;
        }
    }
    
    private string _packingSlipId = string.Empty;
    public string PackingSlipId
    {
        get => _packingSlipId;
        set
        {
            if (_packingSlipId == value)
            {
                return;
            }

            _packingSlipId = value;
            IsChanged = true;
        }
    }
    
    private string _internalPackingSlipId = string.Empty;
    public string InternalPackingSlipId
    {
        get => _internalPackingSlipId;
        set
        {
            if (_internalPackingSlipId == value)
            {
                return;
            }

            _internalPackingSlipId = value;
            IsChanged = true;
        }
    }
    
    private DateTime _transDate = SqlDateTime.MinValue.Value;
    public DateTime TransDate
    {
        get => _transDate;
        set
        {
            if (_transDate == value)
            {
                return;
            }
        
            _transDate = value;
            IsChanged = true;
        }
    }
    
    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set
        {
            if (_description == value)
            {
                return;
            }
        
            _description = value;
            IsChanged = true;
        }
    }
    
    private string _purchId = string.Empty;
    public string PurchId
    {
        get => _purchId;
        set
        {
            if (_purchId == value)
            {
                return;
            }
        
            _purchId = value;
            IsChanged = true;
        }
    }
    
    private string _purchName = string.Empty;
    public string PurchName
    {
        get => _purchName;
        set
        {
            if (_purchName == value)
            {
                return;
            }
        
            _purchName = value;
            IsChanged = true;
        }
    }
    
    private string _orderAccount = string.Empty;
    public string OrderAccount
    {
        get => _orderAccount;
        set
        {
            if (_orderAccount == value)
            {
                return;
            }
        
            _orderAccount = value;
            IsChanged = true;
        }
    }
    
    private string _invoiceAccount = string.Empty;
    public string InvoiceAccount
    {
        get => _invoiceAccount;
        set
        {
            if (_invoiceAccount == value)
            {
                return;
            }
        
            _invoiceAccount = value;
            IsChanged = true;
        }
    }
    
    private string _purchStatus = string.Empty;
    public string PurchStatus
    {
        get => _purchStatus;
        set
        {
            if (_purchStatus == value)
            {
                return;
            }
        
            _purchStatus = value;
            IsChanged = true;
        }
    }
    
    private bool _isSubmitted;
    public bool IsSubmitted
    {
        get => _isSubmitted;
        set
        {
            if (_isSubmitted == value)
            {
                return;
            }
        
            _isSubmitted = value;
            IsChanged = true;
        }
    }
    
    private SubmitStatus _submitStatus = SubmitStatus.Draft;
    public SubmitStatus SubmitStatus
    {
        get => _submitStatus;
        set
        {
            if (_submitStatus == value)
            {
                return;
            }
        
            _submitStatus = value;
            IsChanged = true;
        }
    }
    
    private DateTime _submittedDate = SqlDateTime.MinValue.Value;
    public DateTime SubmittedDate
    {
        get => _submittedDate;
        set
        {
            if (_submittedDate == value)
            {
                return;
            }
        
            _submittedDate = value;
            IsChanged = true;
        }
    }
    
    private string _submittedBy = string.Empty;
    public string SubmittedBy
    {
        get => _submittedBy;
        set
        {
            if (_submittedBy == value)
            {
                return;
            }
        
            _submittedBy = value;
            IsChanged = true;
        }
    }

    public ICollection<GoodsReceiptLine> GoodsReceiptLines { get; set; } = new List<GoodsReceiptLine>();

    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(GoodsReceiptHeaderId)] = _goodsReceiptHeaderId;
        OriginalValues[nameof(PackingSlipId)] = _packingSlipId;
        OriginalValues[nameof(InternalPackingSlipId)] = _internalPackingSlipId;
        OriginalValues[nameof(TransDate)] = _transDate;
        OriginalValues[nameof(Description)] = _description;
        OriginalValues[nameof(PurchId)] = _purchId;
        OriginalValues[nameof(PurchName)] = _purchName;
        OriginalValues[nameof(OrderAccount)] = _orderAccount;
        OriginalValues[nameof(InvoiceAccount)] = _invoiceAccount;
        OriginalValues[nameof(PurchStatus)] = _purchStatus;
        OriginalValues[nameof(IsSubmitted)] = _isSubmitted;
        OriginalValues[nameof(SubmitStatus)] = _submitStatus;
        OriginalValues[nameof(SubmittedDate)] = _submittedDate;
        OriginalValues[nameof(SubmittedBy)] = _submittedBy;
        base.AcceptChanges();
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        _goodsReceiptHeaderId = OriginalValues[nameof(GoodsReceiptHeaderId)] as int? ?? 0;
        _packingSlipId = OriginalValues[nameof(PackingSlipId)] as string ?? "";
        _internalPackingSlipId = OriginalValues[nameof(InternalPackingSlipId)] as string ?? "";
        _transDate = OriginalValues[nameof(TransDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _description = OriginalValues[nameof(Description)] as string ?? "";
        _purchId = OriginalValues[nameof(PurchId)] as string ?? "";
        _purchName = OriginalValues[nameof(PurchName)] as string ?? "";
        _orderAccount = OriginalValues[nameof(OrderAccount)] as string ?? "";
        _invoiceAccount = OriginalValues[nameof(InvoiceAccount)] as string ?? "";
        _purchStatus = OriginalValues[nameof(PurchStatus)] as string ?? "";
        _isSubmitted = OriginalValues[nameof(IsSubmitted)] as bool? ?? false;
        _submitStatus = OriginalValues[nameof(SubmitStatus)] as SubmitStatus? ?? SubmitStatus.Draft;
        _submittedDate = OriginalValues[nameof(SubmittedDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _submittedBy = OriginalValues[nameof(SubmittedBy)] as string ?? "";
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not GoodsReceiptHeader value)
        {
            return;
        }
        
        PackingSlipId = value.PackingSlipId;
        InternalPackingSlipId = value.InternalPackingSlipId;
        TransDate = value.TransDate;
        Description = value.Description;
        PurchId = value.PurchId;
        PurchName = value.PurchName;
        OrderAccount = value.OrderAccount;
        InvoiceAccount = value.InvoiceAccount;
        PurchStatus = value.PurchStatus;
        IsSubmitted = value.IsSubmitted;
        SubmitStatus = value.SubmitStatus;
        SubmittedDate = value.SubmittedDate;
        SubmittedBy = value.SubmittedBy;
    }
}