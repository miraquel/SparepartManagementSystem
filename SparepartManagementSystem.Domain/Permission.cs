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
        base.AcceptChanges();
        
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
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not Permission permission)
        {
            return;
        }

        PermissionName = permission.PermissionName;
        RoleId = permission.RoleId;
        Module = permission.Module;
        Type = permission.Type;
    }
}