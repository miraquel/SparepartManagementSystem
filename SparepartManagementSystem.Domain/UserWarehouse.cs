using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class UserWarehouse : BaseModel
{
    private int _userWarehouseId;
    public int UserWarehouseId
    {
        get => _userWarehouseId;
        set
        {
            if (_userWarehouseId == value)
            {
                return;
            }

            _userWarehouseId = value;
            IsChanged = true;
        }
    }
    
    private int _userId;
    public int UserId
    {
        get => _userId;
        set
        {
            if (_userId == value)
            {
                return;
            }

            _userId = value;
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
    
    private string _inventSiteId = string.Empty;
    public string InventSiteId
    {
        get => _inventSiteId;
        set
        {
            if (_inventSiteId == value)
            {
                return;
            }

            _inventSiteId = value;
            IsChanged = true;
        }
    }
    
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
            {
                return;
            }

            _name = value;
            IsChanged = true;
        }
    }
    
    private bool _isDefault;
    public bool IsDefault
    {
        get => _isDefault;
        set
        {
            if (_isDefault == value)
            {
                return;
            }

            _isDefault = value;
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
        
        OriginalValues[nameof(UserWarehouseId)] = _userWarehouseId;
        OriginalValues[nameof(UserId)] = _userId;
        OriginalValues[nameof(InventLocationId)] = _inventLocationId;
        OriginalValues[nameof(InventSiteId)] = _inventSiteId;
        OriginalValues[nameof(Name)] = _name;
        OriginalValues[nameof(IsDefault)] = _isDefault;
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
        
        _userWarehouseId = OriginalValue(nameof(UserWarehouseId)) as int? ?? 0;
        _userId = OriginalValue(nameof(UserId)) as int? ?? 0;
        _inventLocationId = OriginalValue(nameof(InventLocationId)) as string ?? string.Empty;
        _inventSiteId = OriginalValue(nameof(InventSiteId)) as string ?? string.Empty;
        _name = OriginalValue(nameof(Name)) as string ?? string.Empty;
        _isDefault = OriginalValue(nameof(IsDefault)) as bool? ?? false;
        _createdBy = OriginalValue(nameof(CreatedBy)) as string ?? string.Empty;
        _createdDateTime = OriginalValue(nameof(CreatedDateTime)) as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValue(nameof(ModifiedBy)) as string ?? string.Empty;
        _modifiedDateTime = OriginalValue(nameof(ModifiedDateTime)) as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not UserWarehouse userWarehouse)
        {
            return;
        }

        UserWarehouseId = userWarehouse.UserWarehouseId;
        UserId = userWarehouse.UserId;
        InventLocationId = userWarehouse.InventLocationId;
        InventSiteId = userWarehouse.InventSiteId;
        Name = userWarehouse.Name;
        IsDefault = userWarehouse.IsDefault;
        CreatedBy = userWarehouse.CreatedBy;
        CreatedDateTime = userWarehouse.CreatedDateTime;
        ModifiedBy = userWarehouse.ModifiedBy;
        ModifiedDateTime = userWarehouse.ModifiedDateTime;
    }
}