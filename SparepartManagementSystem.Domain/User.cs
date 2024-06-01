using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class User : BaseModel
{
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
    
    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set
        {
            if (_username == value)
            {
                return;
            }

            _username = value;
            IsChanged = true;
        }
    }
    
    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set
        {
            if (_email == value)
            {
                return;
            }

            _email = value;
            IsChanged = true;
        }
    }
    
    private string _firstName = string.Empty;
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName == value)
            {
                return;
            }

            _firstName = value;
            IsChanged = true;
        }
    }
    
    private string _lastName = string.Empty;
    public string LastName
    {
        get => _lastName;
        set
        {
            if (_lastName == value)
            {
                return;
            }

            _lastName = value;
            IsChanged = true;
        }
    }

    private bool _isAdministrator;
    public bool IsAdministrator
    {
        get => _isAdministrator;
        set
        {
            if (_isAdministrator == value)
            {
                return;
            }

            _isAdministrator = value;
            IsChanged = true;
        }
    }
    
    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled == value)
            {
                return;
            }

            _isEnabled = value;
            IsChanged = true;
        }
    }
    
    private DateTime _lastLogin = SqlDateTime.MinValue.Value;
    public DateTime LastLogin
    {
        get => _lastLogin;
        set
        {
            if (_lastLogin == value)
            {
                return;
            }

            _lastLogin = value;
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
    
    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<UserWarehouse> UserWarehouses { get; set; } = [];
    public override void AcceptChanges()
    {
        if (!IsChanged)
        {
            return;
        }
        
        OriginalValues[nameof(UserId)] = _userId;
        OriginalValues[nameof(Username)] = _username;
        OriginalValues[nameof(Email)] = _email;
        OriginalValues[nameof(FirstName)] = _firstName;
        OriginalValues[nameof(LastName)] = _lastName;
        OriginalValues[nameof(IsAdministrator)] = _isAdministrator;
        OriginalValues[nameof(IsEnabled)] = _isEnabled;
        OriginalValues[nameof(LastLogin)] = _lastLogin;
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
        
        _userId = OriginalValues[nameof(UserId)] as int? ?? 0;
        _username = OriginalValues[nameof(Username)] as string ?? string.Empty;
        _email = OriginalValues[nameof(Email)] as string ?? string.Empty;
        _firstName = OriginalValues[nameof(FirstName)] as string ?? string.Empty;
        _lastName = OriginalValues[nameof(LastName)] as string ?? string.Empty;
        _isAdministrator = OriginalValues[nameof(IsAdministrator)] as bool? ?? false;
        _isEnabled = OriginalValues[nameof(IsEnabled)] as bool? ?? false;
        _lastLogin = OriginalValues[nameof(LastLogin)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _createdBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        _createdDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        
        IsChanged = false;
    }

    public override void UpdateProperties<T>(T source)
    {
        if (source is not User user)
        {
            return;
        }

        UserId = user.UserId;
        Username = user.Username;
        Email = user.Email;
        FirstName = user.FirstName;
        LastName = user.LastName;
        IsAdministrator = user.IsAdministrator;
        IsEnabled = user.IsEnabled;
        LastLogin = user.LastLogin;
        CreatedBy = user.CreatedBy;
        CreatedDateTime = user.CreatedDateTime;
        ModifiedBy = user.ModifiedBy;
        ModifiedDateTime = user.ModifiedDateTime;
    }
}