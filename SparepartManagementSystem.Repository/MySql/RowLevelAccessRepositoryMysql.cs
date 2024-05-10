using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.Interface;

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
    
    public async Task Add(RowLevelAccess entity)
    {
        const string sql = """
                           INSERT INTO RowLevelAccesses
                           (UserId, AxTable, Query, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@UserId, @AxTable, @Query, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { RowLevelAccessId = id }, _dbTransaction);
    }
    public Task<IEnumerable<RowLevelAccess>> GetAll()
    {
        const string sql = "SELECT * FROM RowLevelAccesses";
        return _sqlConnection.QueryAsync<RowLevelAccess>(sql, transaction: _dbTransaction);
    }
    public Task<RowLevelAccess> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId";
        const string sqlForUpdate = "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<RowLevelAccess>(forUpdate ? sqlForUpdate: sql, new { RowLevelAccessId = id }, _dbTransaction);
    }
    public Task<IEnumerable<RowLevelAccess>> GetByParams(RowLevelAccess entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (entity.UserId != 0)
        {
            sqlBuilder.Where("UserId = @UserId", new { entity.UserId });
        }

        if (entity.AxTable != 0)
        {
            sqlBuilder.Where("AxTable LIKE @AxTable", new { AxTable = $"%{entity.AxTable}%" });
        }

        if (!string.IsNullOrEmpty(entity.Query))
        {
            sqlBuilder.Where("Query LIKE @Query", new { Query = $"%{entity.Query}%" });
        }

        const string sql = "SELECT * FROM RowLevelAccesses /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        
        return _sqlConnection.QueryAsync<RowLevelAccess>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(RowLevelAccess entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (entity.UserId != 0)
        {
            sqlBuilder.Set("UserId = @UserId", new { entity.UserId });
        }

        if (entity.AxTable != AxTable.None)
        {
            sqlBuilder.Set("AxTable = @AxTable", new { entity.AxTable });
        }

        if (!string.IsNullOrEmpty(entity.Query))
        {
            sqlBuilder.Set("Query = @Query", new { entity.Query });
        }

        if (!string.IsNullOrEmpty(entity.ModifiedBy))
        {
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.ModifiedDateTime != DateTime.MinValue)
        {
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        sqlBuilder.Where("RowLevelAccessId = @RowLevelAccessId", new { entity.RowLevelAccessId });

        const string sql = "UPDATE RowLevelAccesses /**set**/ /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }
    
    public Task<IEnumerable<RowLevelAccess>> GetByUserId(int userId)
    {
        const string sql = "SELECT * FROM RowLevelAccesses WHERE UserId = @UserId";
        return _sqlConnection.QueryAsync<RowLevelAccess>(sql, new { UserId = userId }, _dbTransaction);
    }
    
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}