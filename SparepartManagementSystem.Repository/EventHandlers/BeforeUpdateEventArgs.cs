using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.EventHandlers;

public class BeforeUpdateEventArgs : EventArgs
{
    public BeforeUpdateEventArgs(BaseModel entity, CustomSqlBuilder sqlBuilder)
    {
        Entity = entity;
        SqlBuilder = sqlBuilder;
    }

    public BaseModel Entity { get; }
    public CustomSqlBuilder SqlBuilder { get; }
}