using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.MySql;

internal class RowLevelAccessRepositoryMysql : IRowLevelAccessRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public RowLevelAccessRepositoryMysql(IDbConnection sqlConnection, IDbTransaction dbTransaction)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task Add(RowLevelAccess entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO RowLevelAccesses
                           (UserId, AxTable, Query, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@UserId, @AxTable, @Query, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId";
        var rows = await _sqlConnection.ExecuteAsync(sql, new { RowLevelAccessId = id }, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Row level access with Id {id} not found");
        }
    }

    public async Task<IEnumerable<RowLevelAccess>> GetAll()
    {
        const string sql = "SELECT * FROM RowLevelAccesses";
        return await _sqlConnection.QueryAsync<RowLevelAccess>(sql, transaction: _dbTransaction);
    }

    public async Task<RowLevelAccess> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId";
        const string sqlForUpdate =
            "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId FOR UPDATE";
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<RowLevelAccess>(forUpdate ? sqlForUpdate : sql,
                new { RowLevelAccessId = id }, _dbTransaction) ??
            throw new InvalidOperationException($"Row level access with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<RowLevelAccess>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("userId", out var userIdString) && int.TryParse(userIdString, out var userId))
        {
            sqlBuilder.Where("UserId = @UserId", new { UserId = userId });
        }

        if (parameters.TryGetValue("axTable", out var axTable) && !string.IsNullOrEmpty(axTable))
        {
            sqlBuilder.Where("AxTable LIKE @AxTable", new { AxTable = $"%{axTable}%" });
        }

        if (parameters.TryGetValue("query", out var query) && !string.IsNullOrEmpty(query))
        {
            sqlBuilder.Where("Query LIKE @Query", new { Query = $"%{query}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }
        
        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) &&
            DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { CreatedDateTime = createdDateTime });
        }
        
        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }
        
        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) &&
            DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { ModifiedDateTime = modifiedDateTime });
        }

        const string sql = "SELECT * FROM RowLevelAccesses /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        return await _sqlConnection.QueryAsync<RowLevelAccess>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(RowLevelAccess entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(RowLevelAccess.UserId)) is not null && !Equals(entity.OriginalValue(nameof(RowLevelAccess.UserId)), entity.UserId))
        {
            builder.Set("UserId = @UserId", new { entity.UserId });
        }

        if (entity.OriginalValue(nameof(RowLevelAccess.AxTable)) is not null && !Equals(entity.OriginalValue(nameof(RowLevelAccess.AxTable)), entity.AxTable))
        {
            builder.Set("AxTable = @AxTable", new { entity.AxTable });
        }

        if (entity.OriginalValue(nameof(RowLevelAccess.Query)) is not null && !Equals(entity.OriginalValue(nameof(RowLevelAccess.Query)), entity.Query))
        {
            builder.Set("Query = @Query", new { entity.Query });
        }
        
        builder.Where("RowLevelAccessId = @RowLevelAccessId", new { entity.RowLevelAccessId });

        if (!builder.HasSet)
        {
            return;
        }
        
        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));

        if (entity.OriginalValue(nameof(RowLevelAccess.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(RowLevelAccess.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(RowLevelAccess.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(RowLevelAccess.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        const string sql = "UPDATE RowLevelAccesses /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Row level access with Id {entity.RowLevelAccessId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
    }

    public async Task<IEnumerable<RowLevelAccess>> GetByUserId(int userId)
    {
        const string sql = "SELECT * FROM RowLevelAccesses WHERE UserId = @UserId";
        return await _sqlConnection.QueryAsync<RowLevelAccess>(sql, new { UserId = userId }, _dbTransaction);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}