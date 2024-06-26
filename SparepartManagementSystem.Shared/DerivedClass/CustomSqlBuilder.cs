using Dapper;

namespace SparepartManagementSystem.Shared.DerivedClass;

public class CustomSqlBuilder : SqlBuilder
{
    public bool HasSet { get; private set; }
    
    public new void Set(string sql, object? parameters = null)
    {
        if (!HasSet)
        {
            HasSet = true;
        }

        base.Set(sql, parameters);
    }
}