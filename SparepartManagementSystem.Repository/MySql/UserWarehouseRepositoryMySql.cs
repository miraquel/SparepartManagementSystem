using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class UserWarehouseRepositoryMySql : IUserWarehouseRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public UserWarehouseRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task Add(UserWarehouse entity)
    {
        const string sql = """
                           INSERT INTO UserWarehouses (UserId, InventLocationId, InventSiteId, Name, IsDefault, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@UserId, @InventLocationId, @InventSiteId, @Name, @IsDefault, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { UserWarehouseId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<UserWarehouse>> GetAll()
    {
        const string sql = "SELECT * FROM UserWarehouses";
        return await _sqlConnection.QueryAsync<UserWarehouse>(sql, transaction: _dbTransaction);
    }

    public Task<UserWarehouse> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId";
        const string sqlForUpdate = "SELECT * FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<UserWarehouse>(forUpdate ? sqlForUpdate : sql, new { UserWarehouseId = id }, _dbTransaction);
    }

    public Task<IEnumerable<UserWarehouse>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();
        
        if (parameters.TryGetValue("UserWarehouseId", out var userWarehouseIdString) && int.TryParse(userWarehouseIdString, out var userWarehouseId))
        {
            builder.Where("UserWarehouseId = @UserWarehouseId", new { UserWarehouseId = userWarehouseId });
        }
        if (parameters.TryGetValue("UserId", out var userIdString) && int.TryParse(userIdString, out var userId))
        {
            builder.Where("UserId = @UserId", new { UserId = userId });
        }
        if (parameters.TryGetValue("InventLocationId", out var inventLocationId) && !string.IsNullOrEmpty(inventLocationId))
        {
            builder.Where("InventLocationId LIKE @InventLocationId", new { InventLocationId = $"%{inventLocationId}%" });
        }
        if (parameters.TryGetValue("InventSiteId", out var inventSiteId) && !string.IsNullOrEmpty(inventSiteId))
        {
            builder.Where("InventSiteId LIKE @InventSiteId", new { InventSiteId = $"%{inventSiteId}%" });
        }
        if (parameters.TryGetValue("Name", out var name) && !string.IsNullOrEmpty(name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"%{name}%" });
        }
        if (parameters.TryGetValue("IsDefault", out var isDefaultString) && bool.TryParse(isDefaultString, out var isDefault))
        {
            builder.Where("IsDefault = @IsDefault", new { IsDefault = isDefault });
        }
        if (parameters.TryGetValue("CreatedBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }
        if (parameters.TryGetValue("CreatedDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { CreatedDateTime = createdDateTime });
        }
        if (parameters.TryGetValue("ModifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }
        if (parameters.TryGetValue("ModifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { ModifiedDateTime = modifiedDateTime });
        }

        const string sql = "SELECT * FROM UserWarehouses /**where**/ /**orderby**/";
        var template = builder.AddTemplate(sql);
        return _sqlConnection.QueryAsync<UserWarehouse>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(UserWarehouse entity)
    {
        var builder = new SqlBuilder();
        
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.UserId)), entity.UserId))
        {
            builder.Set("UserId = @UserId", new { entity.UserId });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.InventLocationId)), entity.InventLocationId))
        {
            builder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.InventSiteId)), entity.InventSiteId))
        {
            builder.Set("InventSiteId = @InventSiteId", new { entity.InventSiteId });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.Name)), entity.Name))
        {
            builder.Set("Name = @Name", new { entity.Name });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.IsDefault)), entity.IsDefault))
        {
            builder.Set("IsDefault = @IsDefault", new { entity.IsDefault });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }
        if (!Equals(entity.OriginalValue(nameof(UserWarehouse.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        builder.Where("UserWarehouseId = @UserWarehouseId", new { entity.UserWarehouseId });
        const string sql = "UPDATE UserWarehouses /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }

    public Task<int> GetLastInsertedId()
    {
        const string sql = "SELECT LAST_INSERT_ID()";
        return _sqlConnection.QueryFirstAsync<int>(sql, transaction: _dbTransaction);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public async Task<IEnumerable<UserWarehouse>> GetByUserId(int userId, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserId = @UserId";
        const string sqlForUpdate = "SELECT * FROM UserWarehouses WHERE UserId = @UserId FOR UPDATE";
        var userWarehouses = await _sqlConnection.QueryAsync<UserWarehouse>(forUpdate ? sqlForUpdate : sql, new { UserId = userId }, _dbTransaction);
        var userWarehousesArray = userWarehouses as UserWarehouse[] ?? userWarehouses.ToArray();
        foreach (var userWarehouse in userWarehousesArray)
        {
            userWarehouse.AcceptChanges();
        }

        return userWarehousesArray;
    }

    public async Task<UserWarehouse> GetDefaultByUserId(int userId)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserId = @UserId AND IsDefault = 1";
        return await _sqlConnection.QueryFirstAsync<UserWarehouse>(sql, new { UserId = userId }, _dbTransaction);
    }
}