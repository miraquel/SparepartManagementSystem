using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class GoodsReceiptHeaderRepositoryMySql : IGoodsReceiptHeaderRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public GoodsReceiptHeaderRepositoryMySql(
        IDbTransaction dbTransaction,
        IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task Add(GoodsReceiptHeader entity)
    {
        const string sql = """
                           INSERT INTO GoodsReceiptHeaders
                           (PackingSlipId, TransDate, Description, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, IsSubmitted, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                           (@PackingSlipId, @TransDate, @Description, @PurchId, @PurchName, @OrderAccount, @InvoiceAccount, @PurchStatus, @IsSubmitted, @SubmittedDate, @SubmittedBy, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { GoodsReceiptHeaderId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<GoodsReceiptHeader>> GetAll()
    {
        const string sql = "SELECT * FROM GoodsReceiptHeaders";
        return await _sqlConnection.QueryAsync<GoodsReceiptHeader>(sql, transaction: _dbTransaction);
    }

    public async Task<GoodsReceiptHeader> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        const string sqlForUpdate = "SELECT * FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId FOR UPDATE";
        var result = await _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(forUpdate ? sqlForUpdate : sql, new { GoodsReceiptHeaderId = id }, _dbTransaction);
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<GoodsReceiptHeader>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (parameters.TryGetValue("goodsReceiptHeaderId", out var goodsReceiptHeaderIdString) && int.TryParse(goodsReceiptHeaderIdString, out var goodsReceiptHeaderId))
        {
            sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { GoodsReceiptHeaderId = goodsReceiptHeaderId });
        }

        if (parameters.TryGetValue("packingSlipId", out var packingSlipId) && !string.IsNullOrEmpty(packingSlipId))
        {
            sqlBuilder.Where("PackingSlipId LIKE @PackingSlipId", new { PackingSlipId = $"%{packingSlipId}%" });
        }

        if (parameters.TryGetValue("transDate", out var transDateString) && DateTime.TryParse(transDateString, out var transDate))
        {
            sqlBuilder.Where("TransDate = @TransDate", new { TransDate = transDate });
        }

        if (parameters.TryGetValue("description", out var description) && !string.IsNullOrEmpty(description))
        {
            sqlBuilder.Where("Description LIKE @Description", new { Description = $"%{description}%" });
        }

        if (parameters.TryGetValue("purchId", out var purchId) && !string.IsNullOrEmpty(purchId))
        {
            sqlBuilder.Where("PurchId LIKE @PurchId", new { PurchId = $"%{purchId}%" });
        }

        if (parameters.TryGetValue("purchName", out var purchName) && !string.IsNullOrEmpty(purchName))
        {
            sqlBuilder.Where("PurchName LIKE @PurchName", new { PurchName = $"%{purchName}%" });
        }

        if (parameters.TryGetValue("orderAccount", out var orderAccount) && !string.IsNullOrEmpty(orderAccount))
        {
            sqlBuilder.Where("OrderAccount LIKE @OrderAccount", new { OrderAccount = $"%{orderAccount}%" });
        }

        if (parameters.TryGetValue("invoiceAccount", out var invoiceAccount) && !string.IsNullOrEmpty(invoiceAccount))
        {
            sqlBuilder.Where("InvoiceAccount LIKE @InvoiceAccount", new { InvoiceAccount = $"%{invoiceAccount}%" });
        }

        if (parameters.TryGetValue("purchStatus", out var purchStatus) && !string.IsNullOrEmpty(purchStatus))
        {
            sqlBuilder.Where("PurchStatus LIKE @PurchStatus", new { PurchStatus = $"%{purchStatus}%" });
        }

        if (parameters.TryGetValue("submittedDate", out var submittedDateString) && DateTime.TryParse(submittedDateString, out var submittedDate))
        {
            sqlBuilder.Where("SubmittedDate = @SubmittedDate", new { SubmittedDate = submittedDate });
        }

        if (parameters.TryGetValue("isSubmitted", out var isSubmittedString) && bool.TryParse(isSubmittedString, out var isSubmitted))
        {
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { IsSubmitted = isSubmitted });
        }

        if (parameters.TryGetValue("submittedBy", out var submittedBy) && !string.IsNullOrEmpty(submittedBy))
        {
            sqlBuilder.Where("SubmittedBy LIKE @SubmittedBy", new { SubmittedBy = $"%{submittedBy}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { ModifiedDateTime = modifiedDateTime });
        }

        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        var template = sqlBuilder.AddTemplate("SELECT * FROM GoodsReceiptHeaders /**where**/");
        return  await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(GoodsReceiptHeader entity)
    {
        var builder = new SqlBuilder();

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.PackingSlipId)), entity.PackingSlipId))
        {
            builder.Set("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.TransDate)), entity.TransDate))
        {
            builder.Set("TransDate = @TransDate", new { entity.TransDate });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.Description)), entity.Description))
        {
            builder.Set("Description = @Description", new { entity.Description });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.PurchId)), entity.PurchId))
        {
            builder.Set("PurchId = @PurchId", new { entity.PurchId });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.PurchName)), entity.PurchName))
        {
            builder.Set("PurchName = @PurchName", new { entity.PurchName });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.OrderAccount)), entity.OrderAccount))
        {
            builder.Set("OrderAccount = @OrderAccount", new { entity.OrderAccount });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.InvoiceAccount)), entity.InvoiceAccount))
        {
            builder.Set("InvoiceAccount = @InvoiceAccount", new { entity.InvoiceAccount });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.PurchStatus)), entity.PurchStatus))
        {
            builder.Set("PurchStatus = @PurchStatus", new { entity.PurchStatus });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.IsSubmitted)), entity.IsSubmitted))
        {
            builder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.SubmittedDate)), entity.SubmittedDate))
        {
            builder.Set("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.SubmittedBy)), entity.SubmittedBy))
        {
            builder.Set("SubmittedBy = @SubmittedBy", new { entity.SubmittedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (!Equals(entity.OriginalValue(nameof(GoodsReceiptHeader.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }
        
        builder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        
        const string sql = "UPDATE GoodsReceiptHeaders /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        entity.AcceptChanges();
    }

    public async Task<PagedList<GoodsReceiptHeader>> GetAllPagedList(int pageNumber, int pageSize)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

        const string sql = "SELECT * FROM GoodsReceiptHeaders /**orderby**/ LIMIT @PageSize OFFSET @Offset";
        var template = sqlBuilder.AddTemplate(sql);
        var result = await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
        const string countSql = "SELECT COUNT(*) FROM GoodsReceiptHeaders";
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(countSql, transaction: _dbTransaction);
        return new PagedList<GoodsReceiptHeader>(result, resultCount, pageNumber, pageSize);
    }

    public async Task<PagedList<GoodsReceiptHeader>> GetByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (parameters.TryGetValue("goodsReceiptHeaderId", out var goodsReceiptHeaderIdString) && int.TryParse(goodsReceiptHeaderIdString, out var goodsReceiptHeaderId))
        {
            sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { GoodsReceiptHeaderId = goodsReceiptHeaderId });
        }

        if (parameters.TryGetValue("packingSlipId", out var packingSlipId) && !string.IsNullOrEmpty(packingSlipId))
        {
            sqlBuilder.Where("PackingSlipId LIKE @PackingSlipId", new { PackingSlipId = $"%{packingSlipId}%" });
        }

        if (parameters.TryGetValue("transDate", out var transDateString) && DateTime.TryParse(transDateString, out var transDate))
        {
            sqlBuilder.Where("TransDate = @TransDate", new { TransDate = transDate });
        }

        if (parameters.TryGetValue("description", out var description) && !string.IsNullOrEmpty(description))
        {
            sqlBuilder.Where("Description LIKE @Description", new { Description = $"%{description}%" });
        }

        if (parameters.TryGetValue("purchId", out var purchId) && !string.IsNullOrEmpty(purchId))
        {
            sqlBuilder.Where("PurchId LIKE @PurchId", new { PurchId = $"%{purchId}%" });
        }

        if (parameters.TryGetValue("purchName", out var purchName) && !string.IsNullOrEmpty(purchName))
        {
            sqlBuilder.Where("PurchName LIKE @PurchName", new { PurchName = $"%{purchName}%" });
        }

        if (parameters.TryGetValue("orderAccount", out var orderAccount) && !string.IsNullOrEmpty(orderAccount))
        {
            sqlBuilder.Where("OrderAccount LIKE @OrderAccount", new { OrderAccount = $"%{orderAccount}%" });
        }

        if (parameters.TryGetValue("invoiceAccount", out var invoiceAccount) && !string.IsNullOrEmpty(invoiceAccount))
        {
            sqlBuilder.Where("InvoiceAccount LIKE @InvoiceAccount", new { InvoiceAccount = $"%{invoiceAccount}%" });
        }

        if (parameters.TryGetValue("purchStatus", out var purchStatus) && !string.IsNullOrEmpty(purchStatus))
        {
            sqlBuilder.Where("PurchStatus LIKE @PurchStatus", new { PurchStatus = $"%{purchStatus}%" });
        }
        
        if (parameters.TryGetValue("isSubmitted", out var isSubmittedString) && bool.TryParse(isSubmittedString, out var isSubmitted))
        {
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { IsSubmitted = isSubmitted });
        }

        if (parameters.TryGetValue("submittedDate", out var submittedDateString) && DateTime.TryParse(submittedDateString, out var submittedDate))
        {
            sqlBuilder.Where("SubmittedDate = @SubmittedDate", new { SubmittedDate = submittedDate });
        }

        if (parameters.TryGetValue("submittedBy", out var submittedBy) && !string.IsNullOrEmpty(submittedBy))
        {
            sqlBuilder.Where("SubmittedBy LIKE @SubmittedBy", new { SubmittedBy = $"%{submittedBy}%" });
        }

        if (parameters.TryGetValue("createdBy", out var createdBy) && !string.IsNullOrEmpty(createdBy))
        {
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{createdBy}%" });
        }

        if (parameters.TryGetValue("createdDateTime", out var createdDateTimeString) && DateTime.TryParse(createdDateTimeString, out var createdDateTime))
        {
            sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { CreatedDateTime = createdDateTime });
        }

        if (parameters.TryGetValue("modifiedBy", out var modifiedBy) && !string.IsNullOrEmpty(modifiedBy))
        {
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{modifiedBy}%" });
        }

        if (parameters.TryGetValue("modifiedDateTime", out var modifiedDateTimeString) && DateTime.TryParse(modifiedDateTimeString, out var modifiedDateTime))
        {
            sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { ModifiedDateTime = modifiedDateTime });
        }

        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });
        
        const string sql = "SELECT * FROM GoodsReceiptHeaders /**where**/ /**orderby**/"; 
        var template = sqlBuilder.AddTemplate(sql);
        var result = await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
        
        const string sqlCount = "SELECT COUNT(*) FROM GoodsReceiptHeaders /**where**/";
        template = sqlBuilder.AddTemplate(sqlCount);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(template.RawSql, template.Parameters, transaction: _dbTransaction);
        
        return new PagedList<GoodsReceiptHeader>(result, pageNumber, pageSize, resultCount);
    }
    public async Task<GoodsReceiptHeader> GetByIdWithLines(int id, bool forUpdate = false)
    {
        // SELECT Statement for GoodsReceiptHeader joined with GoodsReceiptLine
        const string sql = """
                           SELECT
                           grh.*,
                           grl.*
                           FROM GoodsReceiptHeaders grh
                           LEFT JOIN GoodsReceiptLines grl ON grh.GoodsReceiptHeaderId = grl.GoodsReceiptHeaderId
                           WHERE grh.GoodsReceiptHeaderId = @GoodsReceiptHeaderId
                           """;
        
        const string sqlForUpdate = """
                                      SELECT
                                      grh.*,
                                      grl.*
                                      FROM GoodsReceiptHeaders grh
                                      LEFT JOIN GoodsReceiptLines grl ON grh.GoodsReceiptHeaderId = grl.GoodsReceiptHeaderId
                                      WHERE grh.GoodsReceiptHeaderId = @GoodsReceiptHeaderId
                                      FOR UPDATE
                                      """;
        
        var result = new GoodsReceiptHeader();

        _ = await _sqlConnection.QueryAsync<GoodsReceiptHeader, GoodsReceiptLine?, GoodsReceiptHeader>(forUpdate ? sqlForUpdate : sql, (header, line) =>
        {
            if (result.GoodsReceiptHeaderId == 0)
            {
                result = header;
            }

            if (line != null)
            {
                result.GoodsReceiptLines.Add(line);
            }

            return result;
        }, new { GoodsReceiptHeaderId = id }, splitOn: "GoodsReceiptLineId", transaction: _dbTransaction);

        result.AcceptChanges();
        return result;
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}