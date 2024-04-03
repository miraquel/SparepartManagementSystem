using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.Interface;
using static System.String;

namespace SparepartManagementSystem.Repository.MySql;

internal class GoodsReceiptLineRepositoryMySql : IGoodsReceiptLineRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoodsReceiptLineRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection, IHttpContextAccessor httpContextAccessor)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Add(GoodsReceiptLine entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

        const string sql = """
                           INSERT INTO GoodsReceiptLines
                           (GoodsReceiptHeaderId, ItemId, LineNumber, ItemName, ProductType, RemainPurchPhysical, ReceiveNow, PurchQty, PurchUnit, PurchPrice, LineAmount, InventLocationId, WMSLocationId, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                            (@GoodsReceiptHeaderId, @ItemId, @LineNumber, @ItemName, @ProductType, @RemainPurchPhysical, @ReceiveNow, @PurchQty, @PurchUnit, @PurchPrice, @LineAmount, @InventLocationId, @WMSLocationId, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { GoodsReceiptLineId = id }, _dbTransaction);
    }
    public Task<IEnumerable<GoodsReceiptLine>> GetAll()
    {
        const string sql = "SELECT * FROM GoodsReceiptLines";
        return _sqlConnection.QueryAsync<GoodsReceiptLine>(sql, transaction: _dbTransaction);
    }
    public Task<GoodsReceiptLine> GetById(int id)
    {
        const string sql = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId";
        return _sqlConnection.QueryFirstAsync<GoodsReceiptLine>(sql, new { GoodsReceiptLineId = id }, _dbTransaction);
    }
    public Task<IEnumerable<GoodsReceiptLine>> GetByParams(GoodsReceiptLine entity)
    {
        var builder = new SqlBuilder();
        
        if (entity.GoodsReceiptLineId > 0)
            builder.Where("GoodsReceiptLineId = @GoodsReceiptLineId", new { entity.GoodsReceiptLineId });
        if (entity.GoodsReceiptHeaderId > 0)
            builder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!IsNullOrEmpty(entity.ItemId))
            builder.Where("ItemId LIKE @ItemId", new { ItemId = $"%{entity.ItemId}%" });
        if (entity.LineNumber > 0)
            builder.Where("LineNumber = @LineNumber", new { entity.LineNumber });
        if (!IsNullOrEmpty(entity.ItemName))
            builder.Where("ItemName LIKE @ItemName", new { ItemName = $"%{entity.ItemName}%" });
        if (entity.ProductType != ProductType.None)
            builder.Where("ProductType = @ProductType", new { entity.ProductType });
        if (entity.RemainPurchPhysical > 0)
            builder.Where("RemainPurchPhysical = @RemainPurchPhysical", new { entity.RemainPurchPhysical });
        if (entity.ReceiveNow > 0)
            builder.Where("ReceiveNow = @ReceiveNow", new { entity.ReceiveNow });
        if (entity.PurchQty > 0)
            builder.Where("PurchQty = @PurchQty", new { entity.PurchQty });
        if (!IsNullOrEmpty(entity.PurchUnit))
            builder.Where("PurchUnit LIKE @PurchUnit", new { PurchUnit = $"%{entity.PurchUnit}%" });
        if (entity.PurchPrice > 0)
            builder.Where("PurchPrice = @PurchPrice", new { entity.PurchPrice });
        if (entity.LineAmount > 0)
            builder.Where("LineAmount = @LineAmount", new { entity.LineAmount });
        if (!IsNullOrEmpty(entity.InventLocationId))
            builder.Where("InventLocationId LIKE @InventLocationId", new { InventLocationId = $"%{entity.InventLocationId}%" });
        if (!IsNullOrEmpty(entity.WMSLocationId))
            builder.Where("WMSLocationId LIKE @WMSLocationId", new { WMSLocationId = $"%{entity.WMSLocationId}%" });
        if (!IsNullOrEmpty(entity.CreatedBy))
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        if (!IsNullOrEmpty(entity.ModifiedBy))
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
        
        var template = builder.AddTemplate("SELECT * FROM GoodsReceiptLines /**where**/");
        
        return _sqlConnection.QueryAsync<GoodsReceiptLine>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(GoodsReceiptLine entity)
    {
        var builder = new SqlBuilder();
        
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        const string sqlBeforeUpdate = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId FOR UPDATE";
        var beforeUpdate = await _sqlConnection.QueryFirstAsync<GoodsReceiptLine>(sqlBeforeUpdate, new { entity.GoodsReceiptLineId }, _dbTransaction);
        
        if (entity.GoodsReceiptLineId > 0)
            builder.Where("GoodsReceiptLineId = @GoodsReceiptLineId", new { entity.GoodsReceiptLineId });
        if (entity.GoodsReceiptHeaderId != beforeUpdate.GoodsReceiptHeaderId)
            builder.Set("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (entity.ItemId != beforeUpdate.ItemId)
            builder.Set("ItemId = @ItemId", new { entity.ItemId });
        if (entity.LineNumber != beforeUpdate.LineNumber)
            builder.Set("LineNumber = @LineNumber", new { entity.LineNumber });
        if (entity.ItemName != beforeUpdate.ItemName)
            builder.Set("ItemName = @ItemName", new { entity.ItemName });
        if (entity.ProductType != beforeUpdate.ProductType)
            builder.Set("ProductType = @ProductType", new { entity.ProductType });
        if (entity.RemainPurchPhysical != beforeUpdate.RemainPurchPhysical)
            builder.Set("RemainPurchPhysical = @RemainPurchPhysical", new { entity.RemainPurchPhysical });
        if (entity.ReceiveNow != beforeUpdate.ReceiveNow)
            builder.Set("ReceiveNow = @ReceiveNow", new { entity.ReceiveNow });
        if (entity.PurchQty != beforeUpdate.PurchQty)
            builder.Set("PurchQty = @PurchQty", new { entity.PurchQty });
        if (entity.PurchUnit != beforeUpdate.PurchUnit)
            builder.Set("PurchUnit = @PurchUnit", new { entity.PurchUnit });
        if (entity.PurchPrice != beforeUpdate.PurchPrice)
            builder.Set("PurchPrice = @PurchPrice", new { entity.PurchPrice });
        if (entity.LineAmount != beforeUpdate.LineAmount)
            builder.Set("LineAmount = @LineAmount", new { entity.LineAmount });
        if (entity.InventLocationId != beforeUpdate.InventLocationId)
            builder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        if (entity.WMSLocationId != beforeUpdate.WMSLocationId)
            builder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        if (entity.CreatedBy != beforeUpdate.CreatedBy)
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.CreatedDateTime != beforeUpdate.CreatedDateTime)
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        const string sql = """
                           UPDATE GoodsReceiptLines
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
    public async Task<IEnumerable<GoodsReceiptLine>> GetByGoodsReceiptHeaderId(int goodsReceiptHeaderId)
    {
        const string sql = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        return await _sqlConnection.QueryAsync<GoodsReceiptLine>(sql, new { GoodsReceiptHeaderId = goodsReceiptHeaderId }, _dbTransaction);
    }
    
    public async Task<int> BulkAdd(IEnumerable<GoodsReceiptLine> entities)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        
        var dataTable = new DataTable();
        
        var goodsReceiptLineIdColumn = new DataColumn("GoodsReceiptLineId", typeof(int));
        var goodsReceiptHeaderIdColumn = new DataColumn("GoodsReceiptHeaderId", typeof(int));
        var itemIdColumn = new DataColumn("ItemId", typeof(string));
        var lineNumberColumn = new DataColumn("LineNumber", typeof(int));
        var itemNameColumn = new DataColumn("ItemName", typeof(string));
        var productTypeColumn = new DataColumn("ProductType", typeof(int));
        var remainPurchPhysicalColumn = new DataColumn("RemainPurchPhysical", typeof(decimal));
        var receiveNowColumn = new DataColumn("ReceiveNow", typeof(decimal));
        var purchQtyColumn = new DataColumn("PurchQty", typeof(decimal));
        var purchUnitColumn = new DataColumn("PurchUnit", typeof(string));
        var purchPriceColumn = new DataColumn("PurchPrice", typeof(decimal));
        var lineAmountColumn = new DataColumn("LineAmount", typeof(decimal));
        var inventLocationIdColumn = new DataColumn("InventLocationId", typeof(string));
        var wmsLocationIdColumn = new DataColumn("WMSLocationId", typeof(string));
        var createdByColumn = new DataColumn("CreatedBy", typeof(string));
        var createdDateTimeColumn = new DataColumn("CreatedDateTime", typeof(DateTime));
        var modifiedByColumn = new DataColumn("ModifiedBy", typeof(string));
        var modifiedDateTimeColumn = new DataColumn("ModifiedDateTime", typeof(DateTime));
        
        dataTable.Columns.Add(goodsReceiptLineIdColumn);
        dataTable.Columns.Add(goodsReceiptHeaderIdColumn);
        dataTable.Columns.Add(itemIdColumn);
        dataTable.Columns.Add(lineNumberColumn);
        dataTable.Columns.Add(itemNameColumn);
        dataTable.Columns.Add(productTypeColumn);
        dataTable.Columns.Add(remainPurchPhysicalColumn);
        dataTable.Columns.Add(receiveNowColumn);
        dataTable.Columns.Add(purchQtyColumn);
        dataTable.Columns.Add(purchUnitColumn);
        dataTable.Columns.Add(purchPriceColumn);
        dataTable.Columns.Add(lineAmountColumn);
        dataTable.Columns.Add(inventLocationIdColumn);
        dataTable.Columns.Add(wmsLocationIdColumn);
        dataTable.Columns.Add(createdByColumn);
        dataTable.Columns.Add(createdDateTimeColumn);
        dataTable.Columns.Add(modifiedByColumn);
        dataTable.Columns.Add(modifiedDateTimeColumn);
        
        foreach (var entity in entities)
        {
            var row = dataTable.NewRow();
            row[goodsReceiptLineIdColumn] = entity.GoodsReceiptLineId;
            row[goodsReceiptHeaderIdColumn] = entity.GoodsReceiptHeaderId;
            row[itemIdColumn] = entity.ItemId;
            row[lineNumberColumn] = entity.LineNumber;
            row[itemNameColumn] = entity.ItemName;
            row[productTypeColumn] = entity.ProductType;
            row[remainPurchPhysicalColumn] = entity.RemainPurchPhysical;
            row[receiveNowColumn] = entity.ReceiveNow;
            row[purchQtyColumn] = entity.PurchQty;
            row[purchUnitColumn] = entity.PurchUnit;
            row[purchPriceColumn] = entity.PurchPrice;
            row[lineAmountColumn] = entity.LineAmount;
            row[inventLocationIdColumn] = entity.InventLocationId;
            row[wmsLocationIdColumn] = entity.WMSLocationId;
            row[createdByColumn] = currentUser;
            row[createdDateTimeColumn] = DateTime.Now;
            row[modifiedByColumn] = currentUser;
            row[modifiedDateTimeColumn] = DateTime.Now;
            dataTable.Rows.Add(row);
        }
        
        var mySqlConnection = _sqlConnection as MySqlConnection ?? throw new InvalidOperationException("Connection is not initialized");

        mySqlConnection.InfoMessage += (_, args) =>
        {
            foreach (var message in args.Errors)
            {
                Console.WriteLine($"Message: {message.Message}");
            }
        };

        var mySqlBulkCopy = new MySqlBulkCopy(mySqlConnection, _dbTransaction as MySqlTransaction)
        {
            DestinationTableName = "GoodsReceiptLines"
        };

        var result = await mySqlBulkCopy.WriteToServerAsync(dataTable);
        return result.RowsInserted;
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}