using System.Data;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

public class WorkOrderLineRepositoryMySql : IWorkOrderLineRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public WorkOrderLineRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection, IHttpContextAccessor httpContextAccessor)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task Add(WorkOrderLine entity)
    {
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = DateTime.Now;
        
        const string sql = """
                           INSERT INTO WorkOrderLines
                               (WorkOrderLineId, WorkOrderHeaderId, ItemId, ItemName, RequiredDate, Quantity, RequestQuantity, InventLocationId, WMSLocationId, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES 
                               (@WorkOrderLineId, @WorkOrderHeaderId, @ItemId, @ItemName, @RequiredDate, @Quantity, @RequestQuantity, @InventLocationId, @WMSLocationId, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
    public async Task<IEnumerable<WorkOrderLine>> GetAll()
    {
        const string sql = "SELECT * FROM WorkOrderLines";
        return await _sqlConnection.QueryAsync<WorkOrderLine>(sql, transaction: _dbTransaction);
    }
    public Task<WorkOrderLine> GetById(int id)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId";
        return _sqlConnection.QueryFirstAsync<WorkOrderLine>(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }
    public Task<IEnumerable<WorkOrderLine>> GetByParams(WorkOrderLine entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderLineId > 0)
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        if (entity.WorkOrderHeaderId > 0)
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (!string.IsNullOrEmpty(entity.ItemName))
            sqlBuilder.Where("ItemId = @ItemId", new { ItemId = $"%{entity.ItemId}%" });
        if (!string.IsNullOrEmpty(entity.ItemName))
            sqlBuilder.Where("ItemName LIKE @ItemName", new { ItemName = $"%{entity.ItemName}%" });
        if (entity.RequiredDate > DateTime.MinValue)
            sqlBuilder.Where("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        if (entity.Quantity > 0)
            sqlBuilder.Where("Quantity = @Quantity", new { entity.Quantity });
        if (entity.RequestQuantity > 0)
            sqlBuilder.Where("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        if (!string.IsNullOrEmpty(entity.InventLocationId))
            sqlBuilder.Where("InventLocationId = @InventLocationId", new { InventLocationId = $"%{entity.InventLocationId}%" });
        if (!string.IsNullOrEmpty(entity.WMSLocationId))
            sqlBuilder.Where("WMSLocationId = @WMSLocationId", new { WMSLocationId = $"%{entity.WMSLocationId}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy))
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > DateTime.MinValue)
            sqlBuilder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > DateTime.MinValue)
            sqlBuilder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM WorkOrderLines /**where**/");
        
        return _sqlConnection.QueryAsync<WorkOrderLine>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(WorkOrderLine entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderLineId > 0)
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });
        if (entity.WorkOrderHeaderId > 0)
            sqlBuilder.Set("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (!string.IsNullOrEmpty(entity.ItemName))
            sqlBuilder.Set("ItemId = @ItemId", new { entity.ItemId });
        if (!string.IsNullOrEmpty(entity.ItemName))
            sqlBuilder.Set("ItemName = @ItemName", new { entity.ItemName });
        if (entity.RequiredDate > DateTime.MinValue)
            sqlBuilder.Set("RequiredDate = @RequiredDate", new { entity.RequiredDate });
        if (entity.Quantity > 0)
            sqlBuilder.Set("Quantity = @Quantity", new { entity.Quantity });
        if (entity.RequestQuantity > 0)
            sqlBuilder.Set("RequestQuantity = @RequestQuantity", new { entity.RequestQuantity });
        if (!string.IsNullOrEmpty(entity.InventLocationId))
            sqlBuilder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        if (!string.IsNullOrEmpty(entity.WMSLocationId))
            sqlBuilder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        if (!string.IsNullOrEmpty(entity.CreatedBy))
            sqlBuilder.Set("CreatedBy = @CreatedBy", new { entity.CreatedBy });
        if (entity.CreatedDateTime > DateTime.MinValue)
            sqlBuilder.Set("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime > DateTime.MinValue)
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        var template = sqlBuilder.AddTemplate("UPDATE WorkOrderLines /**set**/ /**where**/");
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        const string sql = "SELECT LAST_INSERT_ID()";
        return _sqlConnection.ExecuteScalarAsync<int>(sql, transaction: _dbTransaction);
    }
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public async Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        return await _sqlConnection.QueryAsync<WorkOrderLine>(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }
}