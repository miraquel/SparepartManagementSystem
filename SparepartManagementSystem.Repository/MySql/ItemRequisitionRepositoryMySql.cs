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
        entity.AcceptChanges();
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
    public async Task<IEnumerable<ItemRequisition>> GetAll()
    {
        const string sql = "SELECT * FROM ItemRequisitions";
        return await _sqlConnection.QueryAsync<ItemRequisition>(sql, transaction: _dbTransaction);
    }
    public async Task<ItemRequisition> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        const string sqlForUpdate = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId FOR UPDATE";
        var result = await _sqlConnection.QueryFirstAsync<ItemRequisition>(forUpdate ? sqlForUpdate : sql, new { WorkOrderLineId = id }, _dbTransaction);
        result.AcceptChanges();
        return result;
    }
    public async Task<IEnumerable<ItemRequisition>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("itemRequisitionId", out var itemRequisitionIdString) && int.TryParse(itemRequisitionIdString, out var itemRequisitionId))
        {
            sqlBuilder.Where("ItemRequisitionId = @ItemRequisitionId", new { ItemRequisitionId = itemRequisitionId });
        }
        
        if (parameters.TryGetValue("workOrderLineId", out var workOrderLineIdString) && int.TryParse(workOrderLineIdString, out var workOrderLineId))
        {
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { WorkOrderLineId = workOrderLineId });
        }

        if (parameters.TryGetValue("itemId", out var itemId) && !string.IsNullOrEmpty(itemId))
        {
            sqlBuilder.Where("ItemId LIKE @ItemId", new { ItemId = $"%{itemId}%" });
        }

        if (parameters.TryGetValue("itemName", out var itemName) && !string.IsNullOrEmpty(itemName))
        {
            sqlBuilder.Where("ItemName LIKE @ItemName", new { ItemName = $"%{itemName}%" });
        }

        if (parameters.TryGetValue("requiredDate", out var requiredDateString) && DateTime.TryParse(requiredDateString, out var requiredDate))
        {
            sqlBuilder.Where("RequiredDate = @RequiredDate", new { RequiredDate = requiredDate });
        }

        if (parameters.TryGetValue("quantity", out var quantityString) && int.TryParse(quantityString, out var quantity))
        {
            sqlBuilder.Where("Quantity = @Quantity", new { Quantity = quantity });
        }

        if (parameters.TryGetValue("requestQuantity", out var requestQuantityString) && int.TryParse(requestQuantityString, out var requestQuantity))
        {
            sqlBuilder.Where("RequestQuantity = @RequestQuantity", new { RequestQuantity = requestQuantity });
        }

        if (parameters.TryGetValue("inventLocationId", out var inventLocationId) && !string.IsNullOrEmpty(inventLocationId))
        {
            sqlBuilder.Where("InventLocationId LIKE @InventLocationId", new { InventLocationId = $"%{inventLocationId}%" });
        }

        if (parameters.TryGetValue("wmsLocationId", out var wmsLocationId) && !string.IsNullOrEmpty(wmsLocationId))
        {
            sqlBuilder.Where("WMSLocationId LIKE @WMSLocationId", new { WMSLocationId = $"%{wmsLocationId}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            sqlBuilder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            sqlBuilder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { ModifiedDateTime = modifiedDateTime });
        }
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM ItemRequisitions /**where**/");
        
        return await _sqlConnection.QueryAsync<ItemRequisition>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(ItemRequisition entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.WorkOrderLineId)), entity.WorkOrderLineId))
        {
            sqlBuilder.Set("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.ItemId)), entity.ItemId))
        {
            sqlBuilder.Set("ItemId = @ItemId", new { entity.ItemId });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.ItemName)), entity.ItemName))
        {
            sqlBuilder.Set("ItemName = @ItemName", new { entity.ItemName });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.RequiredDate)), entity.RequiredDate))
        {
            sqlBuilder.Set("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.Quantity)), entity.Quantity))
        {
            sqlBuilder.Set("Quantity = @Quantity", new { entity.Quantity });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.RequestQuantity)), entity.RequestQuantity))
        {
            sqlBuilder.Set("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.InventLocationId)), entity.InventLocationId))
        {
            sqlBuilder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.WMSLocationId)), entity.WMSLocationId))
        {
            sqlBuilder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        }
        
        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.JournalId)), entity.JournalId))
        {
            sqlBuilder.Set("JournalId = @JournalId", new { entity.JournalId });
        }
        
        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.IsSubmitted)), entity.IsSubmitted))
        {
            sqlBuilder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.ModifiedBy)), entity.ModifiedBy))
        {
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(ItemRequisition.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        sqlBuilder.Where("ItemRequisitionId = @ItemRequisitionId", new { entity.ItemRequisitionId });
        var template = sqlBuilder.AddTemplate("UPDATE ItemRequisition /**set**/ /**where**/");
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        entity.AcceptChanges();
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public async Task<IEnumerable<ItemRequisition>> GetByWorkOrderLineId(int id)
    {
        const string sql = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        return await _sqlConnection.QueryAsync<ItemRequisition>(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
}