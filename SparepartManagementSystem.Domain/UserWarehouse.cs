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
        base.AcceptChanges();
        
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
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not UserWarehouse userWarehouse)
        {
            return;
        }

        UserId = userWarehouse.UserId;
        InventLocationId = userWarehouse.InventLocationId;
        InventSiteId = userWarehouse.InventSiteId;
        Name = userWarehouse.Name;
        IsDefault = userWarehouse.IsDefault;
    }
}