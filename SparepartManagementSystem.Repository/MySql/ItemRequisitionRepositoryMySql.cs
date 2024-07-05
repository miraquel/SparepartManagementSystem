using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

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

    public async Task Add(ItemRequisition entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO ItemRequisitions
                               (ItemRequisitionId, WorkOrderLineId, ItemId, ItemName, RequiredDate, Quantity, RequestQuantity, InventLocationId, WMSLocationId, JournalId, IsSubmitted, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES 
                               (@ItemRequisitionId, @WorkOrderLineId, @ItemId, @ItemName, @RequiredDate, @Quantity, @RequestQuantity, @InventLocationId, @WMSLocationId, @JournalId, @IsSubmitted, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;

        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
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
        const string sqlForUpdate =
            "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId FOR UPDATE";
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<ItemRequisition>(forUpdate ? sqlForUpdate : sql,
                new { WorkOrderLineId = id }, _dbTransaction) ??
            throw new Exception($"Item requisition with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<ItemRequisition>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("itemRequisitionId", out var itemRequisitionIdString) &&
            int.TryParse(itemRequisitionIdString, out var itemRequisitionId))
        {
            sqlBuilder.Where("ItemRequisitionId = @ItemRequisitionId", new { ItemRequisitionId = itemRequisitionId });
        }

        if (parameters.TryGetValue("workOrderLineId", out var workOrderLineIdString) &&
            int.TryParse(workOrderLineIdString, out var workOrderLineId))
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

        if (parameters.TryGetValue("requiredDate", out var requiredDateString) &&
            DateTime.TryParse(requiredDateString, out var requiredDate))
        {
            sqlBuilder.Where("RequiredDate = @RequiredDate", new { RequiredDate = requiredDate });
        }

        if (parameters.TryGetValue("quantity", out var quantityString) &&
            int.TryParse(quantityString, out var quantity))
        {
            sqlBuilder.Where("Quantity = @Quantity", new { Quantity = quantity });
        }

        if (parameters.TryGetValue("requestQuantity", out var requestQuantityString) &&
            int.TryParse(requestQuantityString, out var requestQuantity))
        {
            sqlBuilder.Where("RequestQuantity = @RequestQuantity", new { RequestQuantity = requestQuantity });
        }

        if (parameters.TryGetValue("inventLocationId", out var inventLocationId) &&
            !string.IsNullOrEmpty(inventLocationId))
        {
            sqlBuilder.Where("InventLocationId LIKE @InventLocationId",
                new { InventLocationId = $"%{inventLocationId}%" });
        }

        if (parameters.TryGetValue("wmsLocationId", out var wmsLocationId) && !string.IsNullOrEmpty(wmsLocationId))
        {
            sqlBuilder.Where("WMSLocationId LIKE @WMSLocationId", new { WMSLocationId = $"%{wmsLocationId}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) &&
            DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            sqlBuilder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)",
                new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) &&
            DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            sqlBuilder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)",
                new { ModifiedDateTime = modifiedDateTime });
        }

        var template = sqlBuilder.AddTemplate("SELECT * FROM ItemRequisitions /**where**/");

        return await _sqlConnection.QueryAsync<ItemRequisition>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(ItemRequisition entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(ItemRequisition.WorkOrderLineId)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.WorkOrderLineId)), entity.WorkOrderLineId))
        {
            builder.Set("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.ItemId)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.ItemId)), entity.ItemId))
        {
            builder.Set("ItemId = @ItemId", new { entity.ItemId });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.ItemName)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.ItemName)), entity.ItemName))
        {
            builder.Set("ItemName = @ItemName", new { entity.ItemName });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.RequiredDate)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.RequiredDate)), entity.RequiredDate))
        {
            builder.Set("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.Quantity)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.Quantity)), entity.Quantity))
        {
            builder.Set("Quantity = @Quantity", new { entity.Quantity });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.RequestQuantity)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.RequestQuantity)), entity.RequestQuantity))
        {
            builder.Set("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.InventLocationId)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.InventLocationId)), entity.InventLocationId))
        {
            builder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.WMSLocationId)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.WMSLocationId)), entity.WMSLocationId))
        {
            builder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.JournalId)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.JournalId)), entity.JournalId))
        {
            builder.Set("JournalId = @JournalId", new { entity.JournalId });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.IsSubmitted)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.IsSubmitted)), entity.IsSubmitted))
        {
            builder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        }
        
        builder.Where("ItemRequisitionId = @ItemRequisitionId", new { entity.ItemRequisitionId });

        if (!builder.HasSet)
        {
            return;
        }
        
        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));

        if (entity.OriginalValue(nameof(ItemRequisition.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(ItemRequisition.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(ItemRequisition.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        var template = builder.AddTemplate("UPDATE ItemRequisition /**set**/ /**where**/");
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Item requisition with Id {entity.ItemRequisitionId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<IEnumerable<ItemRequisition>> GetByWorkOrderLineId(int id)
    {
        const string sql = "SELECT * FROM ItemRequisitions WHERE WorkOrderLineId = @WorkOrderLineId";
        return await _sqlConnection.QueryAsync<ItemRequisition>(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
}