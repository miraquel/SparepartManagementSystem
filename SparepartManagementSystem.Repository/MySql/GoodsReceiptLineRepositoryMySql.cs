using System.Data;
using Dapper;
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

    public GoodsReceiptLineRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }
    
    public async Task Add(GoodsReceiptLine entity)
    {
        const string sql = """
                           INSERT INTO GoodsReceiptLines
                               (GoodsReceiptHeaderId, ItemId, LineNumber, ItemName, ProductType, RemainPurchPhysical, ReceiveNow, PurchQty, PurchUnit, PurchPrice, LineAmount, InventLocationId, WMSLocationId, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                               (@GoodsReceiptHeaderId, @ItemId, @LineNumber, @ItemName, @ProductType, @RemainPurchPhysical, @ReceiveNow, @PurchQty, @PurchUnit, @PurchPrice, @LineAmount, @InventLocationId, @WMSLocationId, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { GoodsReceiptLineId = id }, _dbTransaction);
    }
    public async Task<IEnumerable<GoodsReceiptLine>> GetAll()
    {
        const string sql = "SELECT * FROM GoodsReceiptLines";
        return await _sqlConnection.QueryAsync<GoodsReceiptLine>(sql, transaction: _dbTransaction);
    }
    public async Task<GoodsReceiptLine> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId";
        const string sqlForUpdate = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptLineId = @GoodsReceiptLineId FOR UPDATE";
        var result = await _sqlConnection.QueryFirstAsync<GoodsReceiptLine>(forUpdate ? sqlForUpdate : sql, new { GoodsReceiptLineId = id }, _dbTransaction);
        result.AcceptChanges();
        return result;
    }
    public async Task<IEnumerable<GoodsReceiptLine>> GetByParams(Dictionary<string, string> parameters)
    {
        var builder = new SqlBuilder();

        if (parameters.TryGetValue("goodsReceiptLineId", out var goodsReceiptLineIdString) && int.TryParse(goodsReceiptLineIdString, out var goodsReceiptLineId))
        {
            builder.Where("GoodsReceiptLineId = @GoodsReceiptLineId", new { GoodsReceiptLineId = goodsReceiptLineId });
        }

        if (parameters.TryGetValue("goodsReceiptHeaderId", out var goodsReceiptHeaderIdString) && int.TryParse(goodsReceiptHeaderIdString, out var goodsReceiptHeaderId))
        {
            builder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { GoodsReceiptHeaderId = goodsReceiptHeaderId });
        }

        if (parameters.TryGetValue("itemId", out var itemId) && !IsNullOrEmpty(itemId))
        {
            builder.Where("ItemId LIKE @ItemId", new { ItemId = $"%{itemId}%" });
        }

        if (parameters.TryGetValue("lineNumber", out var lineNumberString) && int.TryParse(lineNumberString, out var lineNumber))
        {
            builder.Where("LineNumber = @LineNumber", new { LineNumber = lineNumber });
        }

        if (parameters.TryGetValue("itemName", out var itemName) && !IsNullOrEmpty(itemName))
        {
            builder.Where("ItemName LIKE @ItemName", new { ItemName = $"%{itemName}%" });
        }

        if (parameters.TryGetValue("productType", out var productTypeString) && Enum.TryParse<ProductType>(productTypeString, out var productType))
        {
            builder.Where("ProductType = @ProductType", new { ProductType = productType });
        }

        if (parameters.TryGetValue("remainPurchPhysical", out var remainPurchPhysicalString) && decimal.TryParse(remainPurchPhysicalString, out var remainPurchPhysical))
        {
            builder.Where("RemainPurchPhysical = @RemainPurchPhysical", new { RemainPurchPhysical = remainPurchPhysical });
        }

        if (parameters.TryGetValue("receiveNow", out var receiveNowString) && decimal.TryParse(receiveNowString, out var receiveNow))
        {
            builder.Where("ReceiveNow = @ReceiveNow", new { ReceiveNow = receiveNow });
        }

        if (parameters.TryGetValue("purchQty", out var purchQtyString) && decimal.TryParse(purchQtyString, out var purchQty))
        {
            builder.Where("PurchQty = @PurchQty", new { PurchQty = purchQty });
        }

        if (parameters.TryGetValue("purchUnit", out var purchUnit) && !IsNullOrEmpty(purchUnit))
        {
            builder.Where("PurchUnit LIKE @PurchUnit", new { PurchUnit = $"%{purchUnit}%" });
        }

        if (parameters.TryGetValue("purchPrice", out var purchPriceString) && decimal.TryParse(purchPriceString, out var purchPrice))
        {
            builder.Where("PurchPrice = @PurchPrice", new { PurchPrice = purchPrice });
        }

        if (parameters.TryGetValue("lineAmount", out var lineAmountString) && decimal.TryParse(lineAmountString, out var lineAmount))
        {
            builder.Where("LineAmount = @LineAmount", new { LineAmount = lineAmount });
        }

        if (parameters.TryGetValue("inventLocationId", out var inventLocationId) && !IsNullOrEmpty(inventLocationId))
        {
            builder.Where("InventLocationId LIKE @InventLocationId", new { InventLocationId = $"%{inventLocationId}%" });
        }

        if (parameters.TryGetValue("wmsLocationId", out var wmsLocationId) && !IsNullOrEmpty(wmsLocationId))
        {
            builder.Where("WMSLocationId LIKE @WMSLocationId", new { WMSLocationId = $"%{wmsLocationId}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !IsNullOrEmpty(createdBy))
        {
            builder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            builder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !IsNullOrEmpty(modifiedBy))
        {
            builder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            builder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { ModifiedDateTime = modifiedDateTime });
        }
        
        var template = builder.AddTemplate("SELECT * FROM GoodsReceiptLines /**where**/");
        
        return await _sqlConnection.QueryAsync<GoodsReceiptLine>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(GoodsReceiptLine entity)
    {
        var builder = new SqlBuilder();

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.GoodsReceiptHeaderId)), entity.GoodsReceiptHeaderId))
        {
            builder.Set("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ItemId)), entity.ItemId))
        {
            builder.Set("ItemId = @ItemId", new { entity.ItemId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.LineNumber)), entity.LineNumber))
        {
            builder.Set("LineNumber = @LineNumber", new { entity.LineNumber });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ItemName)), entity.ItemName))
        {
            builder.Set("ItemName = @ItemName", new { entity.ItemName });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ProductType)), entity.ProductType))
        {
            builder.Set("ProductType = @ProductType", new { entity.ProductType });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.RemainPurchPhysical)), entity.RemainPurchPhysical))
        {
            builder.Set("RemainPurchPhysical = @RemainPurchPhysical", new { entity.RemainPurchPhysical });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ReceiveNow)), entity.ReceiveNow))
        {
            builder.Set("ReceiveNow = @ReceiveNow", new { entity.ReceiveNow });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.PurchQty)), entity.PurchQty))
        {
            builder.Set("PurchQty = @PurchQty", new { entity.PurchQty });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.PurchUnit)), entity.PurchUnit))
        {
            builder.Set("PurchUnit = @PurchUnit", new { entity.PurchUnit });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.PurchPrice)), entity.PurchPrice))
        {
            builder.Set("PurchPrice = @PurchPrice", new { entity.PurchPrice });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.LineAmount)), entity.LineAmount))
        {
            builder.Set("LineAmount = @LineAmount", new { entity.LineAmount });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.InventLocationId)), entity.InventLocationId))
        {
            builder.Set("InventLocationId = @InventLocationId", new { entity.InventLocationId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.WMSLocationId)), entity.WMSLocationId))
        {
            builder.Set("WMSLocationId = @WMSLocationId", new { entity.WMSLocationId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptLine.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        builder.Where("GoodsReceiptLineId = @GoodsReceiptLineId", new { entity.GoodsReceiptLineId });
        
        const string sql = "UPDATE GoodsReceiptLines /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        entity.AcceptChanges();
    }
    public async Task<IEnumerable<GoodsReceiptLine>> GetByGoodsReceiptHeaderId(int goodsReceiptHeaderId)
    {
        const string sql = "SELECT * FROM GoodsReceiptLines WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        return await _sqlConnection.QueryAsync<GoodsReceiptLine>(sql, new { GoodsReceiptHeaderId = goodsReceiptHeaderId }, _dbTransaction);
    }
    
    public async Task BulkAdd(IEnumerable<GoodsReceiptLine> entities)
    {
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
            row[createdByColumn] = entity.CreatedBy;
            row[createdDateTimeColumn] = DateTime.Now;
            row[modifiedByColumn] = entity.ModifiedBy;
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

        _ = await mySqlBulkCopy.WriteToServerAsync(dataTable);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}