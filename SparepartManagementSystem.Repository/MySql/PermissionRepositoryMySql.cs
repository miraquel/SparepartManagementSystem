using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class PermissionRepositoryMySql : IPermissionRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection, IHttpContextAccessor httpContextAccessor)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task Add(Permission entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

        const string sql = """
                           INSERT INTO Permissions
                           (RoleId, Module, Type, PermissionName, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@RoleId, @Module, @Type, @PermissionName, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
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

    public async Task<Permission> GetById(int id)
    {
        const string sql = """
                           SELECT * FROM Permissions
                           WHERE PermissionId = @PermissionId
                           """;
        return await _sqlConnection.QueryFirstAsync<Permission>(sql, new { PermissionId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<Permission>> GetByParams(Permission entity)
    {
        var builder = new SqlBuilder();

        if (entity.PermissionId > 0)
            builder.Where("PermissionId = @PermissionId", new { entity.PermissionId });
        if (entity.RoleId > 0)
            builder.Where("RoleId = @RoleId", new { entity.RoleId });
        if (!string.IsNullOrEmpty(entity.Module))
            builder.Where("Module LIKE @Module", new { Module = $"%{entity.Module}%" });
        if (!string.IsNullOrEmpty(entity.Type))
            builder.Where("Type LIKE @Type", new { Type = $"%{entity.Type}%" });
        if (!string.IsNullOrEmpty(entity.PermissionName))
            builder.Where("PermissionName LIKE @PermissionName", new { PermissionName = $"%{entity.PermissionName}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy))
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });

        var template = builder.AddTemplate("SELECT * FROM Permissions /**where**/");
        return await _sqlConnection.QueryAsync<Permission>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(Permission entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;

        var builder = new SqlBuilder();

        if (entity.RoleId > 0)
            builder.Set("RoleId = @RoleId", new { entity.RoleId });
        if (!string.IsNullOrEmpty(entity.PermissionName))
            builder.Set("PermissionName = @PermissionName", new { entity.PermissionName });
        if (!string.IsNullOrEmpty(entity.Module))
            builder.Set("Module = @Module", new { entity.Module });
        if (!string.IsNullOrEmpty(entity.Type))
            builder.Set("Type = @Type", new { entity.Type });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });

        builder.Where("PermissionId = @PermissionId", new { entity.PermissionId });

        const string sql = """
                           UPDATE Permissions
                           /**set**/
                           /**where**/
                           """;

        var template = builder.AddTemplate(sql);
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}