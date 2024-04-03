using System.Data;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class RowLevelAccessRepositoryMysql : IRowLevelAccessRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RowLevelAccessRepositoryMysql(IDbConnection sqlConnection, IDbTransaction dbTransaction, IHttpContextAccessor httpContextAccessor)
    {
        _sqlConnection = sqlConnection;
        _dbTransaction = dbTransaction;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Add(RowLevelAccess entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

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
    public Task<RowLevelAccess> GetById(int id)
    {
        const string sql = "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId";
        return _sqlConnection.QueryFirstAsync<RowLevelAccess>(sql, new { RowLevelAccessId = id }, _dbTransaction);
    }
    public Task<IEnumerable<RowLevelAccess>> GetByParams(RowLevelAccess entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.UserId != 0)
            sqlBuilder.Where("UserId = @UserId", new { entity.UserId });
        if (entity.AxTable != 0)
            sqlBuilder.Where("AxTable LIKE @AxTable", new { AxTable = $"%{entity.AxTable}%" });
        if (!string.IsNullOrEmpty(entity.Query))
            sqlBuilder.Where("Query LIKE @Query", new { Query = $"%{entity.Query}%" });
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM RowLevelAccesses /**where**/");
        
        return _sqlConnection.QueryAsync<RowLevelAccess>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(RowLevelAccess entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        const string sqlBeforeUpdate = "SELECT * FROM RowLevelAccesses WHERE RowLevelAccessId = @RowLevelAccessId FOR UPDATE";
        
        var beforeUpdate = await _sqlConnection.QueryFirstAsync<RowLevelAccess>(sqlBeforeUpdate, new { RowLevelAccessId = entity.RowLevelAccessId }, _dbTransaction);
        
        var sqlBuilder = new SqlBuilder();
        
        if (entity.UserId != beforeUpdate.UserId)
            sqlBuilder.Set("UserId = @UserId", new { entity.UserId });
        if (entity.AxTable != beforeUpdate.AxTable)
            sqlBuilder.Set("AxTable = @AxTable", new { entity.AxTable });
        if (entity.Query != beforeUpdate.Query)
            sqlBuilder.Set("Query = @Query", new { entity.Query });
        if (entity.ModifiedBy != beforeUpdate.ModifiedBy)
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime != beforeUpdate.ModifiedDateTime)
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        sqlBuilder.Where("RowLevelAccessId = @RowLevelAccessId", new { entity.RowLevelAccessId });
        
        var template = sqlBuilder.AddTemplate("UPDATE RowLevelAccesses /**set**/ /**where**/");
        
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