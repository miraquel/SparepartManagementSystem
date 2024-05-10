using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

public class ItemRequisitionRepositoryMySql : IItemRequisitionRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    public ItemRequisitionRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }
    public async Task Add(ItemRequisition entity)
    {
        const string sql = """
                           INSERT INTO ItemRequisitions
                               (ItemRequisitionId, WorkOrderLineId, ItemId, ItemName, RequiredDate, Quantity, RequestQuantity, InventLocationId, WMSLocationId, JournalId, IsSubmitted, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES 
                               (@ItemRequisitionId, @WorkOrderLineId, @ItemId, @ItemName, @RequiredDate, @Quantity, @RequestQuantity, @InventLocationId, @WMSLocationId, @JournalId, @IsSubmitted, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
    public Task<IEnumerable<ItemRequisition>> GetAll()
    {
        const string sql = "SELECT * FROM ItemRequisitions";
        return _sqlConnection.QueryAsync<ItemRequisition>(sql, transaction: _dbTransaction);
    }
    public Task<ItemRequisition> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        const string sqlForUpdate = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<ItemRequisition>(forUpdate ? sqlForUpdate : sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
    public Task<IEnumerable<ItemRequisition>> GetByParams(ItemRequisition entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (entity.ItemRequisitionId > 0)
        {
            sqlBuilder.Where("ItemRequisitionId = @ItemRequisitionId", new { entity.ItemRequisitionId });
        }
        
        if (entity.WorkOrderLineId > 0)
        {
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        }

        if (!string.IsNullOrEmpty(entity.ItemName))
        {
            sqlBuilder.Where("ItemId = @ItemId", new { ItemId = $"%{entity.ItemId}%" });
        }

        if (!string.IsNullOrEmpty(entity.ItemName))
        {
            sqlBuilder.Where("ItemName LIKE @ItemName", new { ItemName = $"%{entity.ItemName}%" });
        }

        if (entity.RequiredDate > DateTime.MinValue)
        {
            sqlBuilder.Where("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        }

        if (entity.Quantity > 0)
        {
            sqlBuilder.Where("Quantity = @Quantity", new { entity.Quantity });
        }

        if (entity.RequestQuantity > 0)
        {
            sqlBuilder.Where("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        }

        if (!string.IsNullOrEmpty(entity.InventLocationId))
        {
            sqlBuilder.Where("InventLocationId = @InventLocationId", new { InventLocationId = $"%{entity.InventLocationId}%" });
        }

        if (!string.IsNullOrEmpty(entity.WMSLocationId))
        {
            sqlBuilder.Where("WMSLocationId = @WMSLocationId", new { WMSLocationId = $"%{entity.WMSLocationId}%" });
        }

        if (!string.IsNullOrEmpty(entity.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        }

        if (entity.CreatedDateTime > DateTime.MinValue)
        {
            sqlBuilder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        }

        if (!string.IsNullOrEmpty(entity.ModifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        }

        if (entity.ModifiedDateTime > DateTime.MinValue)
        {
            sqlBuilder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        }
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM ItemRequisitions /**where**/");
        
        return _sqlConnection.QueryAsync<ItemRequisition>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(ItemRequisition entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (entity.ItemRequisitionId > 0)
        {
            sqlBuilder.Where("ItemRequisitionId = @ItemRequisitionId", new { entity.ItemRequisitionId });
        }

        if (entity.WorkOrderLineId > 0)
        {
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        }

        if (!string.IsNullOrEmpty(entity.ItemName))
        {
            sqlBuilder.Set("ItemId = @ItemId", new { entity.ItemId });
        }

        if (!string.IsNullOrEmpty(entity.ItemName))
        {
            sqlBuilder.Set("ItemName = @ItemName", new { entity.ItemName });
        }

        if (entity.RequiredDate > DateTime.MinValue)
        {
            sqlBuilder.Set("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        }

        if (entity.Quantity > 0)
        {
            sqlBuilder.Set("Quantity = @Quantity", new { entity.Quantity });
        }

        if (entity.RequestQuantity > 0)
        {
            sqlBuilder.Set("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        }

        if (!string.IsNullOrEmpty(entity.InventLocationId))
        {
            sqlBuilder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }

        if (!string.IsNullOrEmpty(entity.WMSLocationId))
        {
            sqlBuilder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        }

        if (!string.IsNullOrEmpty(entity.CreatedBy))
        {
            sqlBuilder.Set("CreatedBy = @CreatedBy", new { entity.CreatedBy });
        }

        if (entity.CreatedDateTime > DateTime.MinValue)
        {
            sqlBuilder.Set("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        }

        if (!string.IsNullOrEmpty(entity.ModifiedBy))
        {
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.ModifiedDateTime > DateTime.MinValue)
        {
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        var template = sqlBuilder.AddTemplate("UPDATE ItemRequisition /**set**/ /**where**/");
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        const string sql = "SELECT LAST_INSERT_ID()";
        return _sqlConnection.ExecuteScalarAsync<int>(sql, transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public Task<IEnumerable<ItemRequisition>> GetByWorkOrderLineId(int id)
    {
        const string sql = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        return _sqlConnection.QueryAsync<ItemRequisition>(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
}