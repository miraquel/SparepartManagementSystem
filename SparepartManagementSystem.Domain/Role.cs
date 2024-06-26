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
        base.AcceptChanges();
        
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
        base.RejectChanges();
        
        IsChanged = false;
    }

    public void UpdateProperties<T>(T source)
    {
        if (source is not Role role)
        {
            return;
        }
        
        RoleName = role.RoleName;
        Description = role.Description;
    }

    public Role Clone()
    {
        var role = (Role)MemberwiseClone();
        role.OriginalValues = new Dictionary<string, object>(OriginalValues);
        return role;
    }
}