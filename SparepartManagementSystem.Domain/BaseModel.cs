using System.ComponentModel;
using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class BaseModel : IRevertibleChangeTracking
{
    protected Dictionary<string, object> OriginalValues { get; set; } = new();
    public int OriginalValuesCount => OriginalValues.Count;
    public object? OriginalValue(string name) => OriginalValues.GetValueOrDefault(name);
    public bool IsChanged { get; protected set; }
    public virtual void AcceptChanges()
    {
        OriginalValues[nameof(CreatedBy)] = CreatedBy;
        OriginalValues[nameof(CreatedDateTime)] = CreatedDateTime;
        OriginalValues[nameof(ModifiedBy)] = ModifiedBy;
        OriginalValues[nameof(ModifiedDateTime)] = ModifiedDateTime;
    }
    public virtual void RejectChanges()
    {
        CreatedBy = OriginalValues[nameof(CreatedBy)] as string ?? string.Empty;
        CreatedDateTime = OriginalValues[nameof(CreatedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
        ModifiedBy = OriginalValues[nameof(ModifiedBy)] as string ?? string.Empty;
        ModifiedDateTime = OriginalValues[nameof(ModifiedDateTime)] as DateTime? ?? SqlDateTime.MinValue.Value;
    }
    public bool ValidateUpdate()
    {
        if (this is { IsChanged: true, OriginalValues.Count: 0 })
        {
            throw new InvalidOperationException("Original values are not set. If this entity is initially created, please call AcceptChanges to initialize original values and update the fields.");
        }
        
        return IsChanged;
    }

    // Audit fields
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
}