using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class Permission : BaseModel
{
    private int _permissionId;
    public int PermissionId
    {
        get => _permissionId;
        set
        {
            if (_permissionId == value)
            {
                return;
            }

            _permissionId = value;
            IsChanged = true;
        }
    }
    
    private string _permissionName = string.Empty;
    public string PermissionName
    {
        get => _permissionName;
        set
        {
            if (_permissionName == value)
            {
                return;
            }

            _permissionName = value;
            IsChanged = true;
        }
    }
    
    private int _roleId;
    public int RoleId
    {
        get => _roleId;
        set
        {
            if (_roleId == value)
            {
                return;
            }

            _roleId = value;
            IsChanged = true;
        }
    }
    
    private string _module = string.Empty;
    public string Module
    {
        get => _module;
        set
        {
            if (_module == value)
            {
                return;
            }

            _module = value;
            IsChanged = true;
        }
    }
    
    private string _type = string.Empty;
    public string Type
    {
        get => _type;
        set
        {
            if (_type == value)
            {
                return;
            }

            _type = value;
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
        
        OriginalValues[nameof(PermissionId)] = _permissionId;
        OriginalValues[nameof(PermissionName)] = _permissionName;
        OriginalValues[nameof(RoleId)] = _roleId;
        OriginalValues[nameof(Module)] = _module;
        OriginalValues[nameof(Type)] = _type;
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
        
        _permissionId = (int)OriginalValues[nameof(PermissionId)];
        _permissionName = OriginalValues[nameof(PermissionName)] as string ?? string.Empty;
        _roleId = (int)OriginalValues[nameof(RoleId)];
        _module = OriginalValues[nameof(Module)] as string ?? string.Empty;
        _type = OriginalValues[nameof(Type)] as string ?? string.Empty;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = (DateTime)OriginalValues[nameof(CreatedDateTime)];
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = (DateTime)OriginalValues[nameof(ModifiedDateTime)];
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (!(source is Permission target))
        {
            return;
        }

        PermissionId = target.PermissionId;
        PermissionName = target.PermissionName;
        RoleId = target.RoleId;
        Module = target.Module;
        Type = target.Type;
        CreatedBy = target.CreatedBy;
        CreatedDateTime = target.CreatedDateTime;
        ModifiedBy = target.ModifiedBy;
        ModifiedDateTime = target.ModifiedDateTime;
    }
}