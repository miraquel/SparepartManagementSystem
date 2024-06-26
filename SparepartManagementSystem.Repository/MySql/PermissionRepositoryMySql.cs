using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.MySql;

internal class PermissionRepositoryMySql : IPermissionRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public PermissionRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task<IEnumerable<Permission>> GetByRoleId(int roleId)
    {
        const string sql = """
                           SELECT * FROM Permissions
                           WHERE RoleId = @RoleId
                           """;
        return await _sqlConnection.QueryAsync<Permission>(sql, new { RoleId = roleId },
            _dbTransaction);
    }

    public async Task Add(Permission entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));

        const string sql = """
                           INSERT INTO Permissions
                           (RoleId, Module, Type, PermissionName, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@RoleId, @Module, @Type, @PermissionName, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM Permissions WHERE PermissionId = @PermissionId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { PermissionId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<Permission>> GetAll()
    {
        const string sql = "SELECT * FROM Permissions";
        return await _sqlConnection.QueryAsync<Permission>(sql, transaction: _dbTransaction);
    }

    public async Task<Permission> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM Permissions WHERE PermissionId = @PermissionId";
        const string sqlForUpdate = "SELECT * FROM Permissions WHERE PermissionId = @PermissionId FOR UPDATE";
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<Permission>(forUpdate ? sqlForUpdate : sql,
                new { PermissionId = id }, _dbTransaction) ?? throw new InvalidOperationException($"Permission with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<Permission>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("permissionId", out var permissionIdString) &&
            int.TryParse(permissionIdString, out var permissionId))
        {
            builder.Where("PermissionId = @PermissionId", new { PermissionId = permissionId });
        }

        if (parameters.TryGetValue("roleId", out var roleIdString) && int.TryParse(roleIdString, out var roleId))
        {
            builder.Where("RoleId = @RoleId", new { RoleId = roleId });
        }

        if (parameters.TryGetValue("module", out var module) && !string.IsNullOrEmpty(module))
        {
            builder.Where("Module LIKE @Module", new { Module = $"%{module}%" });
        }

        if (parameters.TryGetValue("type", out var type) && !string.IsNullOrEmpty(type))
        {
            builder.Where("Type LIKE @Type", new { Type = $"%{type}%" });
        }

        if (parameters.TryGetValue("permissionName", out var permissionName) && !string.IsNullOrEmpty(permissionName))
        {
            builder.Where("PermissionName LIKE @PermissionName", new { PermissionName = $"%{permissionName}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) &&
            DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)",
                new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) &&
            DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)",
                new { ModifiedDateTime = modifiedDateTime });
        }

        const string sql = "SELECT * FROM Permissions /**where**/";
        var template = builder.AddTemplate(sql);
        return await _sqlConnection.QueryAsync<Permission>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(Permission entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        onBeforeUpdate?.Invoke(this, new UpdateEventArgs(entity, builder));
        
        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.RoleId)), entity.RoleId))
        {
            builder.Set("RoleId = @RoleId", new { entity.RoleId });
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.PermissionName)), entity.PermissionName))
        {
            builder.Set("PermissionName = @PermissionName", new { entity.PermissionName });
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.Module)), entity.Module))
        {
            builder.Set("Module = @Module", new { entity.Module });
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.Type)), entity.Type))
        {
            builder.Set("Type = @Type", new { entity.Type });
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(Permission.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        builder.Where("PermissionId = @PermissionId", new { entity.PermissionId });
        
        if (!builder.HasSet)
        {
            return;
        }

        const string sql = "UPDATE Permissions /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Permission with Id {entity.PermissionId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new UpdateEventArgs(entity, builder));
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}