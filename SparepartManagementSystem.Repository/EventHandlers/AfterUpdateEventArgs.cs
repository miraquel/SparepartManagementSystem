using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.EventHandlers;

public class AfterUpdateEventArgs : EventArgs
{
    public AfterUpdateEventArgs(BaseModel entity)
    {
        Entity = entity;
    }

    public BaseModel Entity { get; }
}