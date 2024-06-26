using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public abstract class BaseModel : IRevertibleChangeTracking
{
    protected Dictionary<string, object> OriginalValues { get; set; } = new();
    public virtual void AcceptChanges()
    {
        OriginalValues[nameof(CreatedBy)] = CreatedBy;
        OriginalValues[nameof(CreatedDateTime)] = CreatedDateTime;
        OriginalValues[nameof(ModifiedBy)] = _modifiedBy;
        OriginalValues[nameof(ModifiedDateTime)] = _modifiedDateTime;
    }

    public virtual void RejectChanges()
    {
        CreatedBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        CreatedDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        _modifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        _modifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
    }

    public bool IsChanged { get; protected set; }
    public bool ValidateUpdate()
    {
        if (this is { IsChanged: true, OriginalValueLength: 0 })
        {
            throw new InvalidOperationException("Original values are not set. If this entity is initially created, please call AcceptChanges to initialize original values and update the fields.");
        }
        
        return IsChanged;
    }
    public object? OriginalValue(string name) => OriginalValues.GetValueOrDefault(name);
    private int OriginalValueLength => OriginalValues.Count;

    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;

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
}