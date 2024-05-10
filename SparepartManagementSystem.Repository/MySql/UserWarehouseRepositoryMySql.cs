using System.Data;
using System.Data.SqlTypes;
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
                           INSERT INTO UserWarehouses (UserId, InventLocationId, Name, IsDefault, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES (@UserId, @InventLocationId, @Name, @IsDefault, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { UserWarehouseId = id }, _dbTransaction);
    }

    public Task<IEnumerable<UserWarehouse>> GetAll()
    {
        const string sql = "SELECT * FROM UserWarehouses";
        return _sqlConnection.QueryAsync<UserWarehouse>(sql, transaction: _dbTransaction);
    }

    public Task<UserWarehouse> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId";
        const string sqlForUpdate = "SELECT * FROM UserWarehouses WHERE UserWarehouseId = @UserWarehouseId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<UserWarehouse>(forUpdate ? sqlForUpdate : sql, new { UserWarehouseId = id }, _dbTransaction);
    }

    public Task<IEnumerable<UserWarehouse>> GetByParams(UserWarehouse entity)
    {
        var builder = new SqlBuilder();
        
        if (entity.UserWarehouseId != 0)
        {
            builder.Where("UserWarehouseId = @UserWarehouseId", new { entity.UserWarehouseId });
        }
        if (entity.UserId != 0)
        {
            builder.Where("UserId = @UserId", new { entity.UserId });
        }
        if (!string.IsNullOrEmpty(entity.InventLocationId))
        {
            builder.Where("InventLocationId LIKE @InventLocationId", new { InventLocationId = $"%{entity.InventLocationId}%" });
        }
        if (!string.IsNullOrEmpty(entity.Name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"%{entity.Name}%" });
        }
        if (entity.IsDefault != null)
        {
            builder.Where("IsDefault = @IsDefault", new { entity.IsDefault });
        }
        if (!string.IsNullOrEmpty(entity.CreatedBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        }
        if (entity.CreatedDateTime != SqlDateTime.MinValue)
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        }
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        }
        if (entity.ModifiedDateTime != SqlDateTime.MinValue)
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        }

        const string sql = "SELECT * FROM UserWarehouses /**where**/ /**orderby**/";
        var template = builder.AddTemplate(sql);
        return _sqlConnection.QueryAsync<UserWarehouse>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(UserWarehouse entity)
    {
        var builder = new SqlBuilder();
        
        if (entity.UserId != 0)
        {
            builder.Set("UserId = @UserId", new { entity.UserId });
        }
        if (!string.IsNullOrEmpty(entity.InventLocationId))
        {
            builder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }
        if (!string.IsNullOrEmpty(entity.Name))
        {
            builder.Set("Name = @Name", new { entity.Name });
        }
        if (entity.IsDefault != null)
        {
            builder.Set("IsDefault = @IsDefault", new { entity.IsDefault });
        }
        if (!string.IsNullOrEmpty(entity.CreatedBy))
        {
            builder.Set("CreatedBy = @CreatedBy", new { entity.CreatedBy });
        }
        if (entity.CreatedDateTime != SqlDateTime.MinValue)
        {
            builder.Set("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
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
    public Task<IEnumerable<UserWarehouse>> GetByUserId(int userId, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserId = @UserId";
        const string sqlForUpdate = "SELECT * FROM UserWarehouses WHERE UserId = @UserId FOR UPDATE";
        return _sqlConnection.QueryAsync<UserWarehouse>(forUpdate ? sqlForUpdate : sql, new { UserId = userId }, _dbTransaction);
    }

    public async Task<UserWarehouse> GetDefaultByUserId(int userId)
    {
        const string sql = "SELECT * FROM UserWarehouses WHERE UserId = @UserId AND IsDefault = 1";
        return await _sqlConnection.QueryFirstAsync<UserWarehouse>(sql, new { UserId = userId }, _dbTransaction);
    }
}