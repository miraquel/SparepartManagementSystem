using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

public class GoodsReceiptHeaderRepositoryMySql : IGoodsReceiptHeaderRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoodsReceiptHeaderRepositoryMySql(
        IDbTransaction dbTransaction,
        IDbConnection sqlConnection,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GoodsReceiptHeader> Add(GoodsReceiptHeader entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

        _ = await _sqlConnection.ExecuteAsync(
            """
            INSERT INTO GoodsReceiptHeaders
            (PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
            VALUES (@PackingSlipId, @PurchId, @PurchName, @OrderAccount, @InvoiceAccount, @PurchStatus, @SubmittedDate, @SubmittedBy, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
            """,
            entity, _dbTransaction);

        return await GetById(
            await _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction));
    }

    public async Task<GoodsReceiptHeader> Delete(int id)
    {
        var beforeResult = await _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(
            """
            SELECT * FROM GoodsReceiptHeaders
            WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId
            """, new { GoodsReceiptHeaderId = id }, _dbTransaction);

        _ = await _sqlConnection.ExecuteAsync(
            """
            DELETE FROM GoodsReceiptHeaders
            WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId
            """, new { GoodsReceiptHeaderId = id }, _dbTransaction);

        return beforeResult;
    }

    public Task<IEnumerable<GoodsReceiptHeader>> GetAll()
    {
        return _sqlConnection.QueryAsync<GoodsReceiptHeader>(
            """
            SELECT
            GoodsReceiptHeaderId, PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
            FROM GoodsReceiptHeaders
            """, transaction: _dbTransaction);
    }

    public Task<GoodsReceiptHeader> GetById(int id)
    {
        return _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(
            "SELECT GoodsReceiptHeaderId, PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime\r\nFROM GoodsReceiptHeaders\r\nWHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId",
            new { GoodsReceiptHeaderId = id }, _dbTransaction);
    }

    public Task<IEnumerable<GoodsReceiptHeader>> GetByParams(GoodsReceiptHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        if (entity.GoodsReceiptHeaderId > 0) sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!string.IsNullOrEmpty(entity.PackingSlipId)) sqlBuilder.Where("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (!string.IsNullOrEmpty(entity.PurchId)) sqlBuilder.Where("PurchId = @PurchId", new { entity.PurchId });
        if (!string.IsNullOrEmpty(entity.PurchName)) sqlBuilder.Where("PurchName LIKE @PurchName", new { PurchName = $"%{entity.PurchName}%" });
        if (!string.IsNullOrEmpty(entity.OrderAccount)) sqlBuilder.Where("OrderAccount LIKE @OrderAccount", new { OrderAccount = $"%{entity.OrderAccount}%" });
        if (!string.IsNullOrEmpty(entity.InvoiceAccount)) sqlBuilder.Where("InvoiceAccount LIKE @InvoiceAccount", new { InvoiceAccount = $"%{entity.InvoiceAccount}%" });
        if (!string.IsNullOrEmpty(entity.PurchStatus)) sqlBuilder.Where("PurchStatus LIKE @PurchStatus", new { PurchStatus = $"%{entity.PurchStatus}%" });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value) sqlBuilder.Where("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.SubmittedBy)) sqlBuilder.Where("SubmittedBy LIKE @SubmittedBy", new { SubmittedBy = $"%{entity.SubmittedBy}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy)) sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value) sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy)) sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value) sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        var template = sqlBuilder.AddTemplate(
            """
            SELECT GoodsReceiptHeaderId, PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
            FROM GoodsReceiptHeaders /**where**/
            """);
        
        return _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task<GoodsReceiptHeader> Update(GoodsReceiptHeader entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        var builder = new SqlBuilder();
        
        if (entity.GoodsReceiptHeaderId > 0)
            builder.Set("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!string.IsNullOrEmpty(entity.PackingSlipId))
            builder.Set("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (!string.IsNullOrEmpty(entity.PurchId))
            builder.Set("PurchId = @PurchId", new { entity.PurchId });
        if (!string.IsNullOrEmpty(entity.PurchName))
            builder.Set("PurchName = @PurchName", new { entity.PurchName });
        if (!string.IsNullOrEmpty(entity.OrderAccount))
            builder.Set("OrderAccount = @OrderAccount", new { entity.OrderAccount });
        if (!string.IsNullOrEmpty(entity.InvoiceAccount))
            builder.Set("InvoiceAccount = @InvoiceAccount", new { entity.InvoiceAccount });
        if (!string.IsNullOrEmpty(entity.PurchStatus))
            builder.Set("PurchStatus = @PurchStatus", new { entity.PurchStatus });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value)
            builder.Set("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.SubmittedBy))
            builder.Set("SubmittedBy = @SubmittedBy", new { entity.SubmittedBy });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        const string beforeSql = """
                                 SELECT
                                    GoodsReceiptHeaderId, PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
                                 FROM GoodsReceiptHeaders
                                 WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId
                                 """;
        var beforeResult = await _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(beforeSql, new { entity.GoodsReceiptHeaderId }, _dbTransaction);
        
        var template = builder.AddTemplate($"UPDATE GoodsReceiptHeaders SET /**set**/ WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId");
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);

        return beforeResult;
    }

    public async Task<PagedList<GoodsReceiptHeader>> GetAllPagedList(int pageNumber, int pageSize)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM GoodsReceiptHeaders /**orderby**/ LIMIT @PageSize OFFSET @Offset");
        var result = await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM GoodsReceiptHeaders", transaction: _dbTransaction);
        return new PagedList<GoodsReceiptHeader>(result, resultCount, pageNumber, pageSize);
    }

    public async Task<PagedList<GoodsReceiptHeader>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.GoodsReceiptHeaderId > 0)
            sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!string.IsNullOrEmpty(entity.PackingSlipId))
            sqlBuilder.Where("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (!string.IsNullOrEmpty(entity.PurchId))
            sqlBuilder.Where("PurchId = @PurchId", new { entity.PurchId });
        if (!string.IsNullOrEmpty(entity.PurchName))
            sqlBuilder.Where("PurchName LIKE @PurchName", new { PurchName = $"%{entity.PurchName}%" });
        if (!string.IsNullOrEmpty(entity.OrderAccount))
            sqlBuilder.Where("OrderAccount LIKE @OrderAccount", new { OrderAccount = $"%{entity.OrderAccount}%" });
        if (!string.IsNullOrEmpty(entity.InvoiceAccount))
            sqlBuilder.Where("InvoiceAccount LIKE @InvoiceAccount", new { InvoiceAccount = $"%{entity.InvoiceAccount}%" });
        if (!string.IsNullOrEmpty(entity.PurchStatus))
            sqlBuilder.Where("PurchStatus LIKE @PurchStatus", new { PurchStatus = $"%{entity.PurchStatus}%" });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.SubmittedBy))
            sqlBuilder.Where("SubmittedBy LIKE @SubmittedBy", new { SubmittedBy = $"%{entity.SubmittedBy}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy))
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });
        
        var template = sqlBuilder.AddTemplate(
            """
            SELECT
              GoodsReceiptHeaderId, PackingSlipId, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime
            FROM GoodsReceiptHeaders
            /**where**/
            /**orderby**/
            LIMIT @PageSize OFFSET @Offset
            """);
        
        var result = await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM GoodsReceiptHeaders /**where**/");
        return new PagedList<GoodsReceiptHeader>(result, resultCount, pageNumber, pageSize);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}