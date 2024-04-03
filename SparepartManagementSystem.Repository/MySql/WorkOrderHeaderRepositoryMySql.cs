using System.Data;
using System.Data.SqlTypes;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.MySql;

public class WorkOrderHeaderRepositoryMySql : IWorkOrderHeaderRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkOrderHeaderRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection, IHttpContextAccessor httpContextAccessor)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task Add(WorkOrderHeader entity)
    {
        entity.CreatedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.CreatedDateTime = DateTime.Now;
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        const string sql = """
                           INSERT INTO WorkOrderHeaders 
                               (IsSubmitted, SubmittedDate, AGSEAMWOID, AGSEAMWRID, AGSEAMEntityID, Name, HeaderTitle, AGSEAMPriorityID, AGSEAMWOTYPE, AGSEAMWOStatusID, AGSEAMPlanningStartDate, AGSEAMPlanningEndDate, EntityShutDown, WOCloseDate, AGSEAMSuspend, Notes, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES 
                               (@IsSubmitted, @SubmittedDate, @AGSEAMWOID, @AGSEAMWRID, @AGSEAMEntityID, @Name, @HeaderTitle, @AGSEAMPriorityID, @AGSEAMWOTYPE, @AGSEAMWOStatusID, @AGSEAMPlanningStartDate, @AGSEAMPlanningEndDate, @EntityShutDown, @WOCloseDate, @AGSEAMSuspend, @Notes, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }
    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM WorkOrderHeaders WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }
    public Task<IEnumerable<WorkOrderHeader>> GetAll()
    {
        const string sql = "SELECT * FROM WorkOrderHeaders";
        return _sqlConnection.QueryAsync<WorkOrderHeader>(sql, transaction: _dbTransaction);
    }
    public Task<WorkOrderHeader> GetById(int id)
    {
        const string sql = "SELECT * FROM WorkOrderHeaders WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        return _sqlConnection.QueryFirstAsync<WorkOrderHeader>(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }
    public Task<IEnumerable<WorkOrderHeader>> GetByParams(WorkOrderHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderHeaderId != 0)
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (entity.IsSubmitted is not null)
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(SubmittedDate AS date) = CAST(@SubmittedDate AS date)", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOID))
            sqlBuilder.Where("AGSEAMWOID LIKE @AGSEAMWOID", new { AGSEAMWOID = $"%{entity.AGSEAMWOID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWRID))
            sqlBuilder.Where("AGSEAMWRID LIKE @AGSEAMWRID", new { AGSEAMWRID = $"%{entity.AGSEAMWRID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMEntityID))
            sqlBuilder.Where("AGSEAMEntityID LIKE @AGSEAMEntityID", new { AGSEAMEntityID = $"%{entity.AGSEAMEntityID}%" });
        if (!string.IsNullOrEmpty(entity.Name))
            sqlBuilder.Where("Name LIKE @Name", new { Name = $"%{entity.Name}%" });
        if (!string.IsNullOrEmpty(entity.HeaderTitle))
            sqlBuilder.Where("HeaderTitle LIKE @HeaderTitle", new { HeaderTitle = $"%{entity.HeaderTitle}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMPriorityID))
            sqlBuilder.Where("AGSEAMPriorityID LIKE @AGSEAMPriorityID", new { AGSEAMPriorityID = $"%{entity.AGSEAMPriorityID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOTYPE))
            sqlBuilder.Where("AGSEAMWOTYPE LIKE @AGSEAMWOTYPE", new { AGSEAMWOTYPE = $"%{entity.AGSEAMWOTYPE}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOStatusID))
            sqlBuilder.Where("AGSEAMWOStatusID LIKE @AGSEAMWOStatusID", new { AGSEAMWOStatusID = $"%{entity.AGSEAMWOStatusID}%" });
        if (entity.AGSEAMPlanningStartDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(AGSEAMPlanningStartDate AS date) = CAST(@AGSEAMPlanningStartDate AS date)", new { entity.AGSEAMPlanningStartDate });
        if (entity.AGSEAMPlanningEndDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(AGSEAMPlanningEndDate AS date) = CAST(@AGSEAMPlanningEndDate AS date)", new { entity.AGSEAMPlanningEndDate });
        if (entity.EntityShutDown is not NoYes.None)
            sqlBuilder.Where("EntityShutDown = @EntityShutDown", new { entity.EntityShutDown });
        if (entity.WOCloseDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(WOCloseDate AS date) = CAST(@WOCloseDate AS date)", new { entity.WOCloseDate });
        if (entity.AGSEAMSuspend is not NoYes.None)
            sqlBuilder.Where("AGSEAMSuspend = @AGSEAMSuspend", new { entity.AGSEAMSuspend });
        if (!string.IsNullOrEmpty(entity.Notes))
            sqlBuilder.Where("Notes LIKE @Notes", new { Notes = $"%{entity.Notes}%" });
        if (!string.IsNullOrEmpty(entity.CreatedBy))
            sqlBuilder.Where("CreatedBy LIKE @CreatedBy", new { CreatedBy = $"%{entity.CreatedBy}%" });
        if (entity.CreatedDateTime > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(CreatedDateTime AS date) = CAST(@CreatedDateTime AS date)", new { entity.CreatedDateTime });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Where("ModifiedBy LIKE @ModifiedBy", new { ModifiedBy = $"%{entity.ModifiedBy}%" });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(ModifiedDateTime AS date) = CAST(@ModifiedDateTime AS date)", new { entity.ModifiedDateTime });
            
        var template = sqlBuilder.AddTemplate("SELECT * FROM WorkOrderHeaders /**where**/");
        
        return _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);
    }
    public async Task Update(WorkOrderHeader entity)
    {
        entity.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        entity.ModifiedDateTime = DateTime.Now;
        
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderHeaderId > 0)
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (entity.IsSubmitted is not null)
            sqlBuilder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Set("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOID))
            sqlBuilder.Set("AGSEAMWOID = @AGSEAMWOID", new { entity.AGSEAMWOID });
        if (!string.IsNullOrEmpty(entity.AGSEAMWRID))
            sqlBuilder.Set("AGSEAMWRID = @AGSEAMWRID", new { entity.AGSEAMWRID });
        if (!string.IsNullOrEmpty(entity.AGSEAMEntityID))
            sqlBuilder.Set("AGSEAMEntityID = @AGSEAMEntityID", new { entity.AGSEAMEntityID });
        if (!string.IsNullOrEmpty(entity.Name))
            sqlBuilder.Set("Name = @Name", new { entity.Name });
        if (!string.IsNullOrEmpty(entity.HeaderTitle))
            sqlBuilder.Set("HeaderTitle = @HeaderTitle", new { entity.HeaderTitle });
        if (!string.IsNullOrEmpty(entity.AGSEAMPriorityID))
            sqlBuilder.Set("AGSEAMPriorityID = @AGSEAMPriorityID", new { entity.AGSEAMPriorityID });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOTYPE))
            sqlBuilder.Set("AGSEAMWOTYPE = @AGSEAMWOTYPE", new { entity.AGSEAMWOTYPE });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOStatusID))
            sqlBuilder.Set("AGSEAMWOStatusID = @AGSEAMWOStatusID", new { entity.AGSEAMWOStatusID });
        if (entity.AGSEAMPlanningStartDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Set("AGSEAMPlanningStartDate = @AGSEAMPlanningStartDate", new { entity.AGSEAMPlanningStartDate });
        if (entity.AGSEAMPlanningEndDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Set("AGSEAMPlanningEndDate = @AGSEAMPlanningEndDate", new { entity.AGSEAMPlanningEndDate });
        if (entity.EntityShutDown is not NoYes.None)
            sqlBuilder.Set("EntityShutDown = @EntityShutDown", new { entity.EntityShutDown });
        if (entity.WOCloseDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Set("WOCloseDate = @WOCloseDate", new { entity.WOCloseDate });
        if (entity.AGSEAMSuspend is not NoYes.None)
            sqlBuilder.Set("AGSEAMSuspend = @AGSEAMSuspend", new { entity.AGSEAMSuspend });
        if (!string.IsNullOrEmpty(entity.Notes))
            sqlBuilder.Set("Notes = @Notes", new { entity.Notes });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime > SqlDateTime.MinValue.Value)
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        var template = sqlBuilder.AddTemplate("UPDATE WorkOrderHeaders /**set**/ /**where**/");
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }
    public Task<int> GetLastInsertedId()
    {
        const string sql = "SELECT LAST_INSERT_ID()";
        return _sqlConnection.QueryFirstAsync<int>(sql, transaction: _dbTransaction);
    }
    
    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public async Task<PagedList<WorkOrderHeader>> GetAllPagedList(int pageNumber, int pageSize)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.OrderBy("WorkOrderHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

        const string sql = "SELECT * FROM WorkOrderHeaders LIMIT @PageSize OFFSET @Offset";
        var template = sqlBuilder.AddTemplate(sql);
        var result = await _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);
        const string countSql = "SELECT COUNT(*) FROM WorkOrderHeaders";
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(countSql, transaction: _dbTransaction);
        return new PagedList<WorkOrderHeader>(result, pageNumber, pageSize, resultCount);
    }
    public async Task<PagedList<WorkOrderHeader>> GetByParamsPagedList(int pageNumber, int pageSize, WorkOrderHeader entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderHeaderId != 0)
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (entity.IsSubmitted is not null)
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        if (entity.SubmittedDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(SubmittedDate AS date) = CAST(@SubmittedDate AS date)", new { entity.SubmittedDate });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOID))
            sqlBuilder.Where("AGSEAMWOID LIKE @AGSEAMWOID", new { AGSEAMWOID = $"%{entity.AGSEAMWOID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWRID))
            sqlBuilder.Where("AGSEAMWRID LIKE @AGSEAMWRID", new { AGSEAMWRID = $"%{entity.AGSEAMWRID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMEntityID))
            sqlBuilder.Where("AGSEAMEntityID LIKE @AGSEAMEntityID", new { AGSEAMEntityID = $"%{entity.AGSEAMEntityID}%" });
        if (!string.IsNullOrEmpty(entity.Name))
            sqlBuilder.Where("Name LIKE @Name", new { Name = $"%{entity.Name}%" });
        if (!string.IsNullOrEmpty(entity.HeaderTitle))
            sqlBuilder.Where("HeaderTitle LIKE @HeaderTitle", new { HeaderTitle = $"%{entity.HeaderTitle}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMPriorityID))
            sqlBuilder.Where("AGSEAMPriorityID LIKE @AGSEAMPriorityID", new { AGSEAMPriorityID = $"%{entity.AGSEAMPriorityID}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOTYPE))
            sqlBuilder.Where("AGSEAMWOTYPE LIKE @AGSEAMWOTYPE", new { AGSEAMWOTYPE = $"%{entity.AGSEAMWOTYPE}%" });
        if (!string.IsNullOrEmpty(entity.AGSEAMWOStatusID))
            sqlBuilder.Where("AGSEAMWOStatusID LIKE @AGSEAMWOStatusID", new { AGSEAMWOStatusID = $"%{entity.AGSEAMWOStatusID}%" });
        if (entity.AGSEAMPlanningStartDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(AGSEAMPlanningStartDate AS date) = CAST(@AGSEAMPlanningStartDate AS date)", new { entity.AGSEAMPlanningStartDate });
        if (entity.AGSEAMPlanningEndDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(AGSEAMPlanningEndDate AS date) = CAST(@AGSEAMPlanningEndDate AS date)", new { entity.AGSEAMPlanningEndDate });
        if (entity.EntityShutDown is not NoYes.None)
            sqlBuilder.Where("EntityShutDown = @EntityShutDown", new { entity.EntityShutDown });
        if (entity.WOCloseDate > SqlDateTime.MinValue.Value)
            sqlBuilder.Where("CAST(WOCloseDate AS date) = CAST(@WOCloseDate AS date)", new { entity.WOCloseDate });
        if (entity.AGSEAMSuspend is not NoYes.None)
            sqlBuilder.Where("AGSEAMSuspend = @AGSEAMSuspend", new { entity.AGSEAMSuspend });
        if (!string.IsNullOrEmpty(entity.Notes))
            sqlBuilder.Where("Notes LIKE @Notes", new { Notes = $"%{entity.Notes}%" });
        
        sqlBuilder.OrderBy("WorkOrderHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });
        
        const string sql = "SELECT * FROM WorkOrderHeaders /**where**/ LIMIT @PageSize OFFSET @Offset"; 
        var template = sqlBuilder.AddTemplate(sql);
        var result = await _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);
        
        const string sqlCount = "SELECT COUNT(*) FROM WorkOrderHeaders /**where**/";
        template = sqlBuilder.AddTemplate(sqlCount);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(template.RawSql, transaction: _dbTransaction);
        
        return new PagedList<WorkOrderHeader>(result, pageNumber, pageSize, resultCount);
    }
}