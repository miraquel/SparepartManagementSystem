using System.Data;
using Dapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.MySql;

public class WorkOrderHeaderRepositoryMySql : IWorkOrderHeaderRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public WorkOrderHeaderRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task Add(WorkOrderHeader entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO WorkOrderHeaders 
                               (IsSubmitted, SubmittedDate, AGSEAMWOID, AGSEAMWRID, AGSEAMEntityID, Name, HeaderTitle, AGSEAMPriorityID, AGSEAMWOTYPE, AGSEAMWOStatusID, AGSEAMPlanningStartDate, AGSEAMPlanningEndDate, EntityShutDown, WOCloseDate, AGSEAMSuspend, Notes, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES 
                               (@IsSubmitted, @SubmittedDate, @AGSEAMWOID, @AGSEAMWRID, @AGSEAMEntityID, @Name, @HeaderTitle, @AGSEAMPriorityID, @AGSEAMWOTYPE, @AGSEAMWOStatusID, @AGSEAMPlanningStartDate, @AGSEAMPlanningEndDate, @EntityShutDown, @WOCloseDate, @AGSEAMSuspend, @Notes, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime)
                           """;

        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM WorkOrderHeaders WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }

    public async Task<IEnumerable<WorkOrderHeader>> GetAll()
    {
        const string sql = "SELECT * FROM WorkOrderHeaders";
        var workOrderHeaders = await _sqlConnection.QueryAsync<WorkOrderHeader>(sql, transaction: _dbTransaction);
        var workOrderHeadersArray = workOrderHeaders as WorkOrderHeader[] ?? workOrderHeaders.ToArray();
        foreach (var workOrderHeader in workOrderHeadersArray) workOrderHeader.AcceptChanges();
        return workOrderHeadersArray;
    }

    public async Task<WorkOrderHeader> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM WorkOrderHeaders WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        const string sqlForUpdate =
            "SELECT * FROM WorkOrderHeaders WHERE WorkOrderHeaderId = @WorkOrderHeaderId FOR UPDATE";
        var result =
            await _sqlConnection.QueryFirstOrDefaultAsync<WorkOrderHeader>(forUpdate ? sqlForUpdate : sql,
                new { WorkOrderHeaderId = id }, _dbTransaction) ??
            throw new Exception($"Work order header with Id {id} not found");
        result.AcceptChanges();
        return result;
    }

    public async Task<IEnumerable<WorkOrderHeader>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("workOrderHeaderId", out var workOrderHeaderIdString) &&
            int.TryParse(workOrderHeaderIdString, out var workOrderHeaderId))
        {
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { WorkOrderHeaderId = workOrderHeaderId });
        }

        if (parameters.TryGetValue("isSubmitted", out var isSubmittedString) &&
            bool.TryParse(isSubmittedString, out var isSubmitted))
        {
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { IsSubmitted = isSubmitted });
        }

        if (parameters.TryGetValue("submittedDate", out var submittedDateString) &&
            DateTime.TryParse(submittedDateString, out var submittedDate))
        {
            sqlBuilder.Where("CAST(SubmittedDate AS date) = CAST(@SubmittedDate AS date)",
                new { SubmittedDate = submittedDate });
        }

        if (parameters.TryGetValue("aGSEAMWOID", out var agseamwoid) && !string.IsNullOrEmpty(agseamwoid))
        {
            sqlBuilder.Where("AGSEAMWOID LIKE @AGSEAMWOID", new { AGSEAMWOID = $"%{agseamwoid}%" });
        }

        if (parameters.TryGetValue("aGSEAMWRID", out var agseamwrid) && !string.IsNullOrEmpty(agseamwrid))
        {
            sqlBuilder.Where("AGSEAMWRID LIKE @AGSEAMWRID", new { AGSEAMWRID = $"%{agseamwrid}%" });
        }

        if (parameters.TryGetValue("aGSEAMEntityID", out var agseamentityid) && !string.IsNullOrEmpty(agseamentityid))
        {
            sqlBuilder.Where("AGSEAMEntityID LIKE @AGSEAMEntityID", new { AGSEAMEntityID = $"%{agseamentityid}%" });
        }

        if (parameters.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
        {
            sqlBuilder.Where("Name LIKE @Name", new { Name = $"%{name}%" });
        }

        if (parameters.TryGetValue("headerTitle", out var headertitle) && !string.IsNullOrEmpty(headertitle))
        {
            sqlBuilder.Where("HeaderTitle LIKE @HeaderTitle", new { HeaderTitle = $"%{headertitle}%" });
        }

        if (parameters.TryGetValue("aGSEAMPriorityID", out var agseampriorityid) &&
            !string.IsNullOrEmpty(agseampriorityid))
        {
            sqlBuilder.Where("AGSEAMPriorityID LIKE @AGSEAMPriorityID",
                new { AGSEAMPriorityID = $"%{agseampriorityid}%" });
        }

        if (parameters.TryGetValue("aGSEAMWOTYPE", out var agseamwotype) && !string.IsNullOrEmpty(agseamwotype))
        {
            sqlBuilder.Where("AGSEAMWOTYPE LIKE @AGSEAMWOTYPE", new { AGSEAMWOTYPE = $"%{agseamwotype}%" });
        }

        if (parameters.TryGetValue("aGSEAMWOStatusID", out var agseamwostatusid) &&
            !string.IsNullOrEmpty(agseamwostatusid))
        {
            sqlBuilder.Where("AGSEAMWOStatusID LIKE @AGSEAMWOStatusID",
                new { AGSEAMWOStatusID = $"%{agseamwostatusid}%" });
        }

        if (parameters.TryGetValue("aGSEAMPlanningStartDate", out var agseamplanningstartdateString) &&
            DateTime.TryParse(agseamplanningstartdateString, out var agseamplanningstartdate))
        {
            sqlBuilder.Where("CAST(AGSEAMPlanningStartDate AS date) = CAST(@AGSEAMPlanningStartDate AS date)",
                new { AGSEAMPlanningStartDate = agseamplanningstartdate });
        }

        if (parameters.TryGetValue("aGSEAMPlanningEndDate", out var agseamplanningenddateString) &&
            DateTime.TryParse(agseamplanningenddateString, out var agseamplanningenddate))
        {
            sqlBuilder.Where("CAST(AGSEAMPlanningEndDate AS date) = CAST(@AGSEAMPlanningEndDate AS date)",
                new { AGSEAMPlanningEndDate = agseamplanningenddate });
        }

        if (parameters.TryGetValue("entityShutDown", out var entityShutDownString) &&
            Enum.TryParse(entityShutDownString, out NoYes entityShutDown))
        {
            sqlBuilder.Where("EntityShutDown = @EntityShutDown", new { EntityShutDown = entityShutDown });
        }

        if (parameters.TryGetValue("wOCloseDate", out var woclosedateString) &&
            DateTime.TryParse(woclosedateString, out var woclosedate))
        {
            sqlBuilder.Where("CAST(WOCloseDate AS date) = CAST(@WOCloseDate AS date)",
                new { WOCloseDate = woclosedate });
        }

        if (parameters.TryGetValue("aGSEAMSuspend", out var agseamsuspendString) &&
            Enum.TryParse(agseamsuspendString, out NoYes agseamsuspend))
        {
            sqlBuilder.Where("AGSEAMSuspend = @AGSEAMSuspend", new { AGSEAMSuspend = agseamsuspend });
        }

        if (parameters.TryGetValue("notes", out var notes) && !string.IsNullOrEmpty(notes))
        {
            sqlBuilder.Where("Notes LIKE @Notes", new { Notes = $"%{notes}%" });
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

        const string sql = "SELECT * FROM WorkOrderHeaders /**where**/";
        var template = sqlBuilder.AddTemplate(sql);

        return await _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(WorkOrderHeader entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.IsSubmitted)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.IsSubmitted)), entity.IsSubmitted) &&
            entity.IsSubmitted)
        {
            builder.Set("IsSubmitted = @IsSubmitted", new { entity.IsSubmitted });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.SubmittedDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.SubmittedDate)), entity.SubmittedDate))
        {
            builder.Set("SubmittedDate = @SubmittedDate", new { entity.SubmittedDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOID)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOID)), entity.AGSEAMWOID))
        {
            builder.Set("AGSEAMWOID = @AGSEAMWOID", new { entity.AGSEAMWOID });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWRID)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWRID)), entity.AGSEAMWRID))
        {
            builder.Set("AGSEAMWRID = @AGSEAMWRID", new { entity.AGSEAMWRID });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMEntityID)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMEntityID)), entity.AGSEAMEntityID))
        {
            builder.Set("AGSEAMEntityID = @AGSEAMEntityID", new { entity.AGSEAMEntityID });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.Name)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.Name)), entity.Name))
        {
            builder.Set("Name = @Name", new { entity.Name });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.HeaderTitle)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.HeaderTitle)), entity.HeaderTitle))
        {
            builder.Set("HeaderTitle = @HeaderTitle", new { entity.HeaderTitle });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPriorityID)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPriorityID)), entity.AGSEAMPriorityID))
        {
            builder.Set("AGSEAMPriorityID = @AGSEAMPriorityID", new { entity.AGSEAMPriorityID });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOTYPE)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOTYPE)), entity.AGSEAMWOTYPE))
        {
            builder.Set("AGSEAMWOTYPE = @AGSEAMWOTYPE", new { entity.AGSEAMWOTYPE });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOStatusID)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMWOStatusID)), entity.AGSEAMWOStatusID))
        {
            builder.Set("AGSEAMWOStatusID = @AGSEAMWOStatusID", new { entity.AGSEAMWOStatusID });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPlanningStartDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPlanningStartDate)),
                entity.AGSEAMPlanningStartDate))
        {
            builder.Set("AGSEAMPlanningStartDate = @AGSEAMPlanningStartDate",
                new { entity.AGSEAMPlanningStartDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPlanningEndDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMPlanningEndDate)), entity.AGSEAMPlanningEndDate))
        {
            builder.Set("AGSEAMPlanningEndDate = @AGSEAMPlanningEndDate", new { entity.AGSEAMPlanningEndDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.EntityShutDown)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.EntityShutDown)), entity.EntityShutDown))
        {
            builder.Set("EntityShutDown = @EntityShutDown", new { entity.EntityShutDown });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.WOCloseDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.WOCloseDate)), entity.WOCloseDate))
        {
            builder.Set("WOCloseDate = @WOCloseDate", new { entity.WOCloseDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMSuspend)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.AGSEAMSuspend)), entity.AGSEAMSuspend))
        {
            builder.Set("AGSEAMSuspend = @AGSEAMSuspend", new { entity.AGSEAMSuspend });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.Notes)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.Notes)), entity.Notes))
        {
            builder.Set("Notes = @Notes", new { entity.Notes });
        }
        
        builder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });

        if (!builder.HasSet)
        {
            return;
        }
        
        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));

        if (entity.OriginalValue(nameof(WorkOrderHeader.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(WorkOrderHeader.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderHeader.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        const string sql = "UPDATE WorkOrderHeaders /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);

        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Work order header with Id {entity.WorkOrderHeaderId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<PagedList<WorkOrderHeader>> GetAllPagedList(int pageNumber, int pageSize)
    {
        var sqlBuilder = new SqlBuilder();
        sqlBuilder.OrderBy("WorkOrderHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

        const string sql = "SELECT * FROM WorkOrderHeaders LIMIT @PageSize OFFSET @Offset";
        var template = sqlBuilder.AddTemplate(sql);
        var result =
            await _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);
        const string countSql = "SELECT COUNT(*) FROM WorkOrderHeaders";
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(countSql, transaction: _dbTransaction);
        return new PagedList<WorkOrderHeader>(result, pageNumber, pageSize, resultCount);
    }

    public async Task<PagedList<WorkOrderHeader>> GetByParamsPagedList(int pageNumber, int pageSize,
        Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("workOrderHeaderId", out var workOrderHeaderIdString) &&
            int.TryParse(workOrderHeaderIdString, out var workOrderHeaderId))
        {
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { WorkOrderHeaderId = workOrderHeaderId });
        }

        if (parameters.TryGetValue("isSubmitted", out var isSubmittedString) &&
            bool.TryParse(isSubmittedString, out var isSubmitted))
        {
            sqlBuilder.Where("IsSubmitted = @IsSubmitted", new { IsSubmitted = isSubmitted });
        }

        if (parameters.TryGetValue("submittedDate", out var submittedDateString) &&
            DateTime.TryParse(submittedDateString, out var submittedDate))
        {
            sqlBuilder.Where("CAST(SubmittedDate AS date) = CAST(@SubmittedDate AS date)",
                new { SubmittedDate = submittedDate });
        }

        if (parameters.TryGetValue("aGSEAMWOID", out var agseamwoid) && !string.IsNullOrEmpty(agseamwoid))
        {
            sqlBuilder.Where("AGSEAMWOID LIKE @AGSEAMWOID", new { AGSEAMWOID = $"%{agseamwoid}%" });
        }

        if (parameters.TryGetValue("aGSEAMWRID", out var agseamwrid) && !string.IsNullOrEmpty(agseamwrid))
        {
            sqlBuilder.Where("AGSEAMWRID LIKE @AGSEAMWRID", new { AGSEAMWRID = $"%{agseamwrid}%" });
        }

        if (parameters.TryGetValue("aGSEAMEntityID", out var agseamentityid) && !string.IsNullOrEmpty(agseamentityid))
        {
            sqlBuilder.Where("AGSEAMEntityID LIKE @AGSEAMEntityID", new { AGSEAMEntityID = $"%{agseamentityid}%" });
        }

        if (parameters.TryGetValue("name", out var name) && !string.IsNullOrEmpty(name))
        {
            sqlBuilder.Where("Name LIKE @Name", new { Name = $"%{name}%" });
        }

        if (parameters.TryGetValue("headerTitle", out var headerTitle) && !string.IsNullOrEmpty(headerTitle))
        {
            sqlBuilder.Where("HeaderTitle LIKE @HeaderTitle", new { HeaderTitle = $"%{headerTitle}%" });
        }

        if (parameters.TryGetValue("aGSEAMPriorityID", out var agseampriorityid) &&
            !string.IsNullOrEmpty(agseampriorityid))
        {
            sqlBuilder.Where("AGSEAMPriorityID LIKE @AGSEAMPriorityID",
                new { AGSEAMPriorityID = $"%{agseampriorityid}%" });
        }

        if (parameters.TryGetValue("aGSEAMWOTYPE", out var agseamwotype) && !string.IsNullOrEmpty(agseamwotype))
        {
            sqlBuilder.Where("AGSEAMWOTYPE LIKE @AGSEAMWOTYPE", new { AGSEAMWOTYPE = $"%{agseamwotype}%" });
        }

        if (parameters.TryGetValue("aGSEAMWOStatusID", out var agseamwostatusid) &&
            !string.IsNullOrEmpty(agseamwostatusid))
        {
            sqlBuilder.Where("AGSEAMWOStatusID LIKE @AGSEAMWOStatusID",
                new { AGSEAMWOStatusID = $"%{agseamwostatusid}%" });
        }

        if (parameters.TryGetValue("aGSEAMPlanningStartDate", out var agseamplanningstartdateString) &&
            DateTime.TryParse(agseamplanningstartdateString, out var agseamplanningstartdate))
        {
            sqlBuilder.Where("CAST(AGSEAMPlanningStartDate AS date) = CAST(@AGSEAMPlanningStartDate AS date)",
                new { AGSEAMPlanningStartDate = agseamplanningstartdate });
        }

        if (parameters.TryGetValue("aGSEAMPlanningEndDate", out var agseamplanningenddateString) &&
            DateTime.TryParse(agseamplanningenddateString, out var agseamplanningenddate))
        {
            sqlBuilder.Where("CAST(AGSEAMPlanningEndDate AS date) = CAST(@AGSEAMPlanningEndDate AS date)",
                new { AGSEAMPlanningEndDate = agseamplanningenddate });
        }

        if (parameters.TryGetValue("entityShutDown", out var entityShutDownString) &&
            Enum.TryParse(entityShutDownString, out NoYes entityShutDown))
        {
            sqlBuilder.Where("EntityShutDown = @EntityShutDown", new { EntityShutDown = entityShutDown });
        }

        if (parameters.TryGetValue("wOCloseDate", out var woclosedateString) &&
            DateTime.TryParse(woclosedateString, out var woclosedate))
        {
            sqlBuilder.Where("CAST(WOCloseDate AS date) = CAST(@WOCloseDate AS date)",
                new { WOCloseDate = woclosedate });
        }

        if (parameters.TryGetValue("aGSEAMSuspend", out var agseamsuspendString) &&
            Enum.TryParse(agseamsuspendString, out NoYes agseamsuspend))
        {
            sqlBuilder.Where("AGSEAMSuspend = @AGSEAMSuspend", new { AGSEAMSuspend = agseamsuspend });
        }

        if (parameters.TryGetValue("notes", out var notes) && !string.IsNullOrEmpty(notes))
        {
            sqlBuilder.Where("Notes LIKE @Notes", new { Notes = $"%{notes}%" });
        }

        sqlBuilder.OrderBy("WorkOrderHeaderId DESC");
        sqlBuilder.AddParameters(new { PageSize = pageSize, Offset = (pageNumber - 1) * pageSize });

        const string sql = "SELECT * FROM WorkOrderHeaders /**where**/ LIMIT @PageSize OFFSET @Offset";
        var template = sqlBuilder.AddTemplate(sql);
        var result =
            await _sqlConnection.QueryAsync<WorkOrderHeader>(template.RawSql, template.Parameters, _dbTransaction);

        const string sqlCount = "SELECT COUNT(*) FROM WorkOrderHeaders /**where**/";
        template = sqlBuilder.AddTemplate(sqlCount);
        var resultCount = await _sqlConnection.ExecuteScalarAsync<int>(template.RawSql, transaction: _dbTransaction);

        return new PagedList<WorkOrderHeader>(result, pageNumber, pageSize, resultCount);
    }

    public async Task<WorkOrderHeader> GetByIdWithLines(int id)
    {
        const string sql = """
                           SELECT
                           woh.*,
                           wol.*
                           FROM WorkOrderHeaders woh
                           LEFT JOIN WorkOrderLines wol ON woh.WorkOrderHeaderId = wol.WorkOrderHeaderId
                           WHERE woh.WorkOrderHeaderId = @WorkOrderHeaderId
                           """;

        var result = new WorkOrderHeader();

        _ = await _sqlConnection.QueryAsync<WorkOrderHeader, WorkOrderLine?, WorkOrderHeader>(sql, (woh, wol) =>
        {
            if (result.WorkOrderHeaderId == 0)
            {
                result = woh;
            }

            if (wol != null)
            {
                result.WorkOrderLines.Add(wol);
            }

            return result;
        }, new { WorkOrderHeaderId = id }, splitOn: "WorkOrderLineId", transaction: _dbTransaction);

        return result;
    }
}