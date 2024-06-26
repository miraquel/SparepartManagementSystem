using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.EventHandlers;

public sealed class AddEventArgs : EventArgs
{
    public AddEventArgs(BaseModel entity)
    {
        Entity = entity;
    }

    public BaseModel Entity { get; }
}