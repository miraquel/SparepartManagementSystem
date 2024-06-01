using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class Role : BaseModel
{
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
    
    private string _roleName = string.Empty;
    public string RoleName
    {
        get => _roleName;
        set
        {
            if (_roleName == value)
            {
                return;
            }

            _roleName = value;
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
    
    public List<User> Users { get; set; } = [];
    
    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(RoleId)] = _roleId;
        OriginalValues[nameof(RoleName)] = _roleName;
        OriginalValues[nameof(Description)] = _description;
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
        
        _roleId = OriginalValues[nameof(RoleId)] as int? ?? 0;
        _roleName = OriginalValues[nameof(RoleName)] as string ?? string.Empty;
        _description = OriginalValues[nameof(Description)] as string ?? string.Empty;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not Role role)
        {
            return;
        }

        RoleId = role.RoleId;
        RoleName = role.RoleName;
        Description = role.Description;
        CreatedBy = role.CreatedBy;
        CreatedDateTime = role.CreatedDateTime;
        ModifiedBy = role.ModifiedBy;
        ModifiedDateTime = role.ModifiedDateTime;
        
        IsChanged = false;
    }
}