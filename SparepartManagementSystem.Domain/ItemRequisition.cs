using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class ItemRequisition : BaseModel
{
    private int _itemRequisitionId;
    public int ItemRequisitionId
    {
        get => _itemRequisitionId;
        set
        {
            if (_itemRequisitionId == value)
            {
                return;
            }

            _itemRequisitionId = value;
            IsChanged = true;
        }
    }
    
    private int _workOrderLineId;
    public int WorkOrderLineId
    {
        get => _workOrderLineId;
        set
        {
            if (_workOrderLineId == value)
            {
                return;
            }

            _workOrderLineId = value;
            IsChanged = true;
        }
    }
    
    private string _itemId = string.Empty;
    public string ItemId
    {
        get => _itemId;
        set
        {
            if (_itemId == value)
            {
                return;
            }

            _itemId = value;
            IsChanged = true;
        }
    }
    
    private string _itemName = string.Empty;
    public string ItemName
    {
        get => _itemName;
        set
        {
            if (_itemName == value)
            {
                return;
            }

            _itemName = value;
            IsChanged = true;
        }
    }
    
    private DateTime _requiredDate = SqlDateTime.MinValue.Value;
    public DateTime RequiredDate
    {
        get => _requiredDate;
        set
        {
            if (_requiredDate == value)
            {
                return;
            }

            _requiredDate = value;
            IsChanged = true;
        }
    }
    
    private decimal _quantity;
    public decimal Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity == value)
            {
                return;
            }

            _quantity = value;
            IsChanged = true;
        }
    }
    
    private decimal _requestQuantity;
    public decimal RequestQuantity
    {
        get => _requestQuantity;
        set
        {
            if (_requestQuantity == value)
            {
                return;
            }

            _requestQuantity = value;
            IsChanged = true;
        }
    }
    
    private string _inventLocationId = string.Empty;
    public string InventLocationId
    {
        get => _inventLocationId;
        set
        {
            if (_inventLocationId == value)
            {
                return;
            }

            _inventLocationId = value;
            IsChanged = true;
        }
    }
    
    private string _wMsLocationId = string.Empty;
    public string WMSLocationId
    {
        get => _wMsLocationId;
        set
        {
            if (_wMsLocationId == value)
            {
                return;
            }

            _wMsLocationId = value;
            IsChanged = true;
        }
    }
    
    private string _journalId = string.Empty;
    public string JournalId
    {
        get => _journalId;
        set
        {
            if (_journalId == value)
            {
                return;
            }

            _journalId = value;
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
    
    private string _createdBy = string.Empty;
    public string CreatedBy
    {
        get => _createdBy;
        set
        {
            if (_createdBy == value)
            {
                return;
            }

            _createdBy = value;
            IsChanged = true;
        }
    }
    
    private DateTime _createdDateTime = SqlDateTime.MinValue.Value;
    public DateTime CreatedDateTime
    {
        get => _createdDateTime;
        set
        {
            if (_createdDateTime == value)
            {
                return;
            }

            _createdDateTime = value;
            IsChanged = true;
        }
    }
    
    private string _modifiedBy = string.Empty;
    public string ModifiedBy
    {
        get => _modifiedBy;
        set
        {
            if (_modifiedBy == value)
            {
                return;
            }

            _modifiedBy = value;
            IsChanged = true;
        }
    }
    
    private DateTime _modifiedDateTime = SqlDateTime.MinValue.Value;
    public DateTime ModifiedDateTime
    {
        get => _modifiedDateTime;
        set
        {
            if (_modifiedDateTime == value)
            {
                return;
            }

            _modifiedDateTime = value;
            IsChanged = true;
        }
    }
    
    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(ItemRequisitionId)] = _itemRequisitionId;
        OriginalValues[nameof(WorkOrderLineId)] = _workOrderLineId;
        OriginalValues[nameof(ItemId)] = _itemId;
        OriginalValues[nameof(ItemName)] = _itemName;
        OriginalValues[nameof(RequiredDate)] = _requiredDate;
        OriginalValues[nameof(Quantity)] = _quantity;
        OriginalValues[nameof(RequestQuantity)] = _requestQuantity;
        OriginalValues[nameof(InventLocationId)] = _inventLocationId;
        OriginalValues[nameof(WMSLocationId)] = _wMsLocationId;
        OriginalValues[nameof(JournalId)] = _journalId;
        OriginalValues[nameof(IsSubmitted)] = _isSubmitted;
        OriginalValues[nameof(CreatedBy)] = _createdBy;
        OriginalValues[nameof(CreatedDateTime)] = _createdDateTime;
        OriginalValues[nameof(ModifiedBy)] = _modifiedBy;
        OriginalValues[nameof(ModifiedDateTime)] = _modifiedDateTime;
        
        IsChanged = false;
    }

    public override void RejectChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        _itemRequisitionId = OriginalValues[nameof(ItemRequisitionId)] as int? ?? 0;
        _workOrderLineId = OriginalValues[nameof(WorkOrderLineId)] as int? ?? 0;
        _itemId = OriginalValues[nameof(ItemId)] as string ?? string.Empty;
        _itemName = OriginalValues[nameof(ItemName)] as string ?? string.Empty;
        _requiredDate = OriginalValues[nameof(RequiredDate)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _quantity = OriginalValues[nameof(Quantity)] as decimal? ?? 0;
        _requestQuantity = OriginalValues[nameof(RequestQuantity)] as decimal? ?? 0;
        _inventLocationId = OriginalValues[nameof(InventLocationId)] as string ?? string.Empty;
        _wMsLocationId = OriginalValues[nameof(WMSLocationId)] as string ?? string.Empty;
        _journalId = OriginalValues[nameof(JournalId)] as string ?? string.Empty;
        _isSubmitted = OriginalValues[nameof(IsSubmitted)] as bool? ?? false;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not ItemRequisition value)
        {
            return;
        }
        
        ItemRequisitionId = value.ItemRequisitionId;
        WorkOrderLineId = value.WorkOrderLineId;
        ItemId = value.ItemId;
        ItemName = value.ItemName;
        RequiredDate = value.RequiredDate;
        Quantity = value.Quantity;
        RequestQuantity = value.RequestQuantity;
        InventLocationId = value.InventLocationId;
        WMSLocationId = value.WMSLocationId;
        JournalId = value.JournalId;
        IsSubmitted = value.IsSubmitted;
        CreatedBy = value.CreatedBy;
        CreatedDateTime = value.CreatedDateTime;
        ModifiedBy = value.ModifiedBy;
        ModifiedDateTime = value.ModifiedDateTime;
    }
}