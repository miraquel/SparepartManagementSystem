using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

internal class GoodsReceiptHeaderRepositoryMySql : IGoodsReceiptHeaderRepository
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

    public async Task Add(GoodsReceiptHeader entity)
    {
        var currentDateTime = DateTime.Now;
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = currentDateTime;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = currentDateTime;

        const string sql = """
                           INSERT INTO GoodsReceiptHeaders
                           (PackingSlipId, TransDate, Description, PurchId, PurchName, OrderAccount, InvoiceAccount, PurchStatus, IsSubmitted, SubmittedDate, SubmittedBy, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                           (@PackingSlipId, @TransDate, @Description, @PurchId, @PurchName, @OrderAccount, @InvoiceAccount, @PurchStatus, @IsSubmitted, @SubmittedDate, @SubmittedBy, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { GoodsReceiptHeaderId = id }, _dbTransaction);
    }

    public Task<IEnumerable<GoodsReceiptHeader>> GetAll()
    {
        const string sql = "SELECT * FROM GoodsReceiptHeaders";
        return _sqlConnection.QueryAsync<GoodsReceiptHeader>(sql, transaction: _dbTransaction);
    }

    public Task<GoodsReceiptHeader> GetById(int id)
    {
        const string sql = "SELECT * FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId";
        return _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(sql, new { GoodsReceiptHeaderId = id }, _dbTransaction);
    }

    public Task<IEnumerable<GoodsReceiptHeader>> GetByParams(GoodsReceiptHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        if (entity.GoodsReceiptHeaderId > 0) sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!string.IsNullOrEmpty(entity.PackingSlipId)) sqlBuilder.Where("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (entity.TransDate > SqlDateTime.MinValue.Value) sqlBuilder.Where("TransDate = @TransDate", new { entity.TransDate });
        if (!string.IsNullOrEmpty(entity.Description)) sqlBuilder.Where("Description = @Description", new { entity.Description });
        if (!string.IsNullOrEmpty(entity.PurchId)) sqlBuilder.Where("PurchId = @PurchId", new { entity.PurchId });
        if (!string.IsNullOrEmpty(entity.PurchName)) sqlBuilder.Where("PurchName LIKE @PurchName", new { PurchName = $"%{entity.PurchName}%" });
        if (!string.IsNullOrEmpty(entity.OrderAccount)) sqlBuilder.Where("OrderAccount LIKE @OrderAccount", new { OrderAccount = $"%{entity.OrderAccount}%" });
        if (!string.IsNullOrEmpty(entity.InvoiceAccount)) sqlBuilder.Where("InvoiceAccount LIKE @InvoiceAccount", new { InvoiceAccount = $"%{entity.InvoiceAccount}%" });
        if (!string.IsNullOrEmpty(entity.PurchStatus)) sqlBuilder.Where("PurchStatus LIKE @PurchStatus", new { PurchStatus = $"%{entity.PurchStatus}%" });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value) sqlBuilder.Where("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (entity.IsSubmitted is not null) sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        if (!string.IsNullOrEmpty(entity.SubmittedBy)) sqlBuilder.Where("SubmittedBy LIKE @SubmittedBy", new { SubmittedBy = $"%{entity.SubmittedBy}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy)) sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value) sqlBuilder.Where("CreatedDateTime = @CreatedDateTime", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy)) sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value) sqlBuilder.Where("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        sqlBuilder.OrderBy("GoodsReceiptHeaderId DESC");
        
        var template = sqlBuilder.AddTemplate("SELECT * FROM GoodsReceiptHeaders /**where**/");
        
        return _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(GoodsReceiptHeader entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        const string sqlBeforeUpdate = "SELECT * FROM GoodsReceiptHeaders WHERE GoodsReceiptHeaderId = @GoodsReceiptHeaderId FOR UPDATE";
        var beforeUpdate = await _sqlConnection.QueryFirstAsync<GoodsReceiptHeader>(sqlBeforeUpdate, new { entity.GoodsReceiptHeaderId }, _dbTransaction);
        
        var builder = new SqlBuilder();
        
        if (entity.GoodsReceiptHeaderId > 0)
            builder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (entity.PackingSlipId != beforeUpdate.PackingSlipId)
            builder.Set("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (entity.TransDate != beforeUpdate.TransDate)
            builder.Set("TransDate = @TransDate", new { entity.TransDate });
        if (entity.Description != beforeUpdate.Description)
            builder.Set("Description = @Description", new { entity.Description });
        if (entity.PurchId != beforeUpdate.PurchId)
            builder.Set("PurchId = @PurchId", new { entity.PurchId });
        if (entity.PurchName != beforeUpdate.PurchName)
            builder.Set("PurchName = @PurchName", new { entity.PurchName });
        if (entity.OrderAccount != beforeUpdate.OrderAccount)
            builder.Set("OrderAccount = @OrderAccount", new { entity.OrderAccount });
        if (entity.InvoiceAccount != beforeUpdate.InvoiceAccount)
            builder.Set("InvoiceAccount = @InvoiceAccount", new { entity.InvoiceAccount });
        if (entity.PurchStatus != beforeUpdate.PurchStatus)
            builder.Set("PurchStatus = @PurchStatus", new { entity.PurchStatus });
        if (entity.IsSubmitted != beforeUpdate.IsSubmitted)
            builder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        if (entity.SubmittedDate != beforeUpdate.SubmittedDate)
            builder.Set("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (entity.SubmittedBy != beforeUpdate.SubmittedBy)
            builder.Set("SubmittedBy = @SubmittedBy", new { entity.SubmittedBy });
        if (entity.CreatedBy != beforeUpdate.CreatedBy)
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.CreatedDateTime != beforeUpdate.CreatedDateTime)
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        const string sql = "UPDATE GoodsReceiptHeaders /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        return _sqlConnection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID()", transaction: _dbTransaction);
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

    public async Task<PagedList<GoodsReceiptHeader>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.GoodsReceiptHeaderId > 0)
            sqlBuilder.Where("GoodsReceiptHeaderId = @GoodsReceiptHeaderId", new { entity.GoodsReceiptHeaderId });
        if (!string.IsNullOrEmpty(entity.PackingSlipId))
            sqlBuilder.Where("PackingSlipId = @PackingSlipId", new { entity.PackingSlipId });
        if (entity.TransDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("TransDate = @TransDate", new { entity.TransDate });
        if (!string.IsNullOrEmpty(entity.Description))
            sqlBuilder.Where("Description = @Description", new { entity.Description });
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
        if (entity.IsSubmitted is not null)
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
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
        
        const string sql = "SELECT * FROM GoodsReceiptHeaders /**where**/ /**orderby**/"; 
        var template = sqlBuilder.AddTemplate(sql);
        var result = await _sqlConnection.QueryAsync<GoodsReceiptHeader>(template.RawSql, template.Parameters, _dbTransaction);
        
        const string sqlCount = "SELECT COUNT(*) FROM GoodsReceiptHeaders /**where**/";
        template = sqlBuilder.AddTemplate(sqlCount);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(template.RawSql, transaction: _dbTransaction);
        
        return new PagedList<GoodsReceiptHeader>(result, pageNumber, pageSize, resultCount);
    }
    public async Task<GoodsReceiptHeader> GetByIdWithLines(int id)
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
        
        var result = new GoodsReceiptHeader();

        _ = await _sqlConnection.QueryAsync<GoodsReceiptHeader, GoodsReceiptLine?, GoodsReceiptHeader>(sql, (header, line) =>
        {
            if (result.GoodsReceiptHeaderId == 0)
            {
                result = header;
            }

            if (line != null) result.GoodsReceiptLines.Add(line);
            return result;
        }, new { GoodsReceiptHeaderId = id }, splitOn: "GoodsReceiptLineId", transaction: _dbTransaction);

        return result;
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
}