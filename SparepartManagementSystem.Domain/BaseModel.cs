using System.ComponentModel;

namespace SparepartManagementSystem.Domain;

public abstract class BaseModel : IRevertibleChangeTracking
{
    protected Dictionary<string, object> OriginalValues { get; } = new();
    public abstract void AcceptChanges();
    public abstract void RejectChanges();
    public bool IsChanged { get; protected set; }
    public abstract void UpdateProperties<T>(T source);

    public object OriginalValue(string name)
    {
        return OriginalValues[name];
    }
}