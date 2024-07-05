using System.Data;
using Dapper;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Shared.DerivedClass;

namespace SparepartManagementSystem.Repository.MySql;

public class WorkOrderLineRepositoryMySql : IWorkOrderLineRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _sqlConnection;

    public WorkOrderLineRepositoryMySql(IDbTransaction dbTransaction, IDbConnection sqlConnection)
    {
        _dbTransaction = dbTransaction;
        _sqlConnection = sqlConnection;
    }

    public async Task Add(WorkOrderLine entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
        
        const string sql = """
                           INSERT INTO WorkOrderLines
                               (WorkOrderHeaderId, Line, LineTitle, EntityId, EntityShutdown, WorkOrderType, TaskId, "Condition", PlanningStartDate, PlanningEndDate, Supervisor, CalendarId, WorkOrderStatus, Suspend, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                               (@WorkOrderHeaderId, @Line, @LineTitle, @EntityId, @EntityShutdown, @WorkOrderType, @TaskId, @Condition, @PlanningStartDate, @PlanningEndDate, @Supervisor, @CalendarId, @WorkOrderStatus, @Suspend, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime);
                           """;

        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
        entity.AcceptChanges();
        
        onAfterAdd?.Invoke(this, new AddEventArgs(entity));
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

    public async Task<WorkOrderLine> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId";
        const string sqlForUpdate = "SELECT * FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId FOR UPDATE";
        return await _sqlConnection.QueryFirstOrDefaultAsync<WorkOrderLine>(forUpdate ? sqlForUpdate : sql,
                   new { WorkOrderLineId = id }, _dbTransaction) ??
               throw new Exception($"Work order line with Id {id} not found");
    }

    public async Task<IEnumerable<WorkOrderLine>> GetByParams(Dictionary<string, string> parameters)
    {
        var sqlBuilder = new SqlBuilder();

        if (parameters.TryGetValue("workOrderLineId", out var workOrderLineIdString) &&
            int.TryParse(workOrderLineIdString, out var workOrderLineId))
        {
            sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { WorkOrderLineId = workOrderLineId });
        }

        if (parameters.TryGetValue("workOrderHeaderId", out var workOrderHeaderIdString) &&
            int.TryParse(workOrderHeaderIdString, out var workOrderHeaderId))
        {
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { WorkOrderHeaderId = workOrderHeaderId });
        }

        if (parameters.TryGetValue("line", out var lineString) && int.TryParse(lineString, out var line))
        {
            sqlBuilder.Where("Line = @Line", new { Line = line });
        }

        if (parameters.TryGetValue("lineTitle", out var lineTitle) && !string.IsNullOrEmpty(lineTitle))
        {
            sqlBuilder.Where("LineTitle LIKE @LineTitle", new { LineTitle = $"%{lineTitle}%" });
        }

        if (parameters.TryGetValue("entityId", out var entityId) && !string.IsNullOrEmpty(entityId))
        {
            sqlBuilder.Where("EntityId LIKE @EntityId", new { EntityId = $"%{entityId}%" });
        }

        if (parameters.TryGetValue("entityShutdown", out var entityShutdownString) &&
            Enum.TryParse<NoYes>(entityShutdownString, out var entityShutdown))
        {
            sqlBuilder.Where("EntityShutdown = @EntityShutdown", new { EntityShutdown = entityShutdown });
        }

        if (parameters.TryGetValue("workOrderType", out var workOrderType) && !string.IsNullOrEmpty(workOrderType))
        {
            sqlBuilder.Where("WorkOrderType LIKE @WorkOrderType", new { WorkOrderType = $"%{workOrderType}%" });
        }

        if (parameters.TryGetValue("taskId", out var taskId) && !string.IsNullOrEmpty(taskId))
        {
            sqlBuilder.Where("TaskId LIKE @TaskId", new { TaskId = $"%{taskId}%" });
        }

        if (parameters.TryGetValue("condition", out var condition) && !string.IsNullOrEmpty(condition))
        {
            sqlBuilder.Where("Condition LIKE @Condition", new { Condition = $"%{condition}%" });
        }

        if (parameters.TryGetValue("planningStartDate", out var planningStartDateString) &&
            DateTime.TryParse(planningStartDateString, out var planningStartDate))
        {
            sqlBuilder.Where("PlanningStartDate = @PlanningStartDate", new { PlanningStartDate = planningStartDate });
        }

        if (parameters.TryGetValue("planningEndDate", out var planningEndDateString) &&
            DateTime.TryParse(planningEndDateString, out var planningEndDate))
        {
            sqlBuilder.Where("PlanningEndDate = @PlanningEndDate", new { PlanningEndDate = planningEndDate });
        }

        if (parameters.TryGetValue("supervisor", out var supervisor) && !string.IsNullOrEmpty(supervisor))
        {
            sqlBuilder.Where("Supervisor LIKE @Supervisor", new { Supervisor = $"%{supervisor}%" });
        }

        if (parameters.TryGetValue("calendarId", out var calendarId) && !string.IsNullOrEmpty(calendarId))
        {
            sqlBuilder.Where("CalendarId LIKE @CalendarId", new { CalendarId = $"%{calendarId}%" });
        }

        if (parameters.TryGetValue("workOrderStatus", out var workOrderStatus) &&
            !string.IsNullOrEmpty(workOrderStatus))
        {
            sqlBuilder.Where("WorkOrderStatus LIKE @WorkOrderStatus", new { WorkOrderStatus = $"%{workOrderStatus}%" });
        }

        if (parameters.TryGetValue("suspend", out var suspendString) &&
            Enum.TryParse<NoYes>(suspendString, out var suspend))
        {
            sqlBuilder.Where("Suspend = @Suspend", new { Suspend = suspend });
        }

        const string sql = "SELECT * FROM WorkOrderLines /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        return await _sqlConnection.QueryAsync<WorkOrderLine>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(WorkOrderLine entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null)
    {
        var builder = new CustomSqlBuilder();

        if (!entity.ValidateUpdate())
        {
            return;
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.WorkOrderHeaderId)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.WorkOrderHeaderId)), entity.WorkOrderHeaderId))
        {
            builder.Set("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.Line)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.Line)), entity.Line))
        {
            builder.Set("Line = @Line", new { entity.Line });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.LineTitle)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.LineTitle)), entity.LineTitle))
        {
            builder.Set("LineTitle = @LineTitle", new { entity.LineTitle });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.EntityId)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.EntityId)), entity.EntityId))
        {
            builder.Set("EntityId = @EntityId", new { entity.EntityId });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.EntityShutdown)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.EntityShutdown)), entity.EntityShutdown))
        {
            builder.Set("EntityShutdown = @EntityShutdown", new { entity.EntityShutdown });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.WorkOrderType)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.WorkOrderType)), entity.WorkOrderType))
        {
            builder.Set("WorkOrderType = @WorkOrderType", new { entity.WorkOrderType });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.TaskId)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.TaskId)), entity.TaskId))
        {
            builder.Set("TaskId = @TaskId", new { entity.TaskId });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.Condition)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.Condition)), entity.Condition))
        {
            builder.Set("Condition = @Condition", new { entity.Condition });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.PlanningStartDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.PlanningStartDate)), entity.PlanningStartDate))
        {
            builder.Set("PlanningStartDate = @PlanningStartDate", new { entity.PlanningStartDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.PlanningEndDate)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.PlanningEndDate)), entity.PlanningEndDate))
        {
            builder.Set("PlanningEndDate = @PlanningEndDate", new { entity.PlanningEndDate });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.Supervisor)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.Supervisor)), entity.Supervisor))
        {
            builder.Set("Supervisor = @Supervisor", new { entity.Supervisor });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.CalendarId)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.CalendarId)), entity.CalendarId))
        {
            builder.Set("CalendarId = @CalendarId", new { entity.CalendarId });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.WorkOrderStatus)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.WorkOrderStatus)), entity.WorkOrderStatus))
        {
            builder.Set("WorkOrderStatus = @WorkOrderStatus", new { entity.WorkOrderStatus });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.Suspend)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.Suspend)), entity.Suspend))
        {
            builder.Set("Suspend = @Suspend", new { entity.Suspend });
        }
        
        builder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });

        if (!builder.HasSet)
        {
            return;
        }
        
        onBeforeUpdate?.Invoke(this, new BeforeUpdateEventArgs(entity, builder));

        if (entity.OriginalValue(nameof(WorkOrderLine.ModifiedBy)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.ModifiedBy)), entity.ModifiedBy))
        {
            builder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        }

        if (entity.OriginalValue(nameof(WorkOrderLine.ModifiedDateTime)) is not null && !Equals(entity.OriginalValue(nameof(WorkOrderLine.ModifiedDateTime)), entity.ModifiedDateTime))
        {
            builder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        }

        const string sql = "UPDATE WorkOrderLines /**set**/ /**where**/";
        var template = builder.AddTemplate(sql);
        var rows = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
        if (rows == 0)
        {
            throw new InvalidOperationException($"Work order line with Id {entity.WorkOrderLineId} not found");
        }
        entity.AcceptChanges();
        
        onAfterUpdate?.Invoke(this, new AfterUpdateEventArgs(entity));
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;

    public async Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        return await _sqlConnection.QueryAsync<WorkOrderLine>(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }

    public async Task BulkAdd(IEnumerable<WorkOrderLine> entities, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null)
    {
        var dataTable = new DataTable();

        var workOrderLineIdColumn = new DataColumn("WorkOrderLineId", typeof(int));
        var workOrderHeaderIdColumn = new DataColumn("WorkOrderHeaderId", typeof(int));
        var lineColumn = new DataColumn("Line", typeof(int));
        var lineTitleColumn = new DataColumn("LineTitle", typeof(string));
        var entityIdColumn = new DataColumn("EntityId", typeof(string));
        var entityShutdownColumn = new DataColumn("EntityShutdown", typeof(int));
        var workOrderTypeColumn = new DataColumn("WorkOrderType", typeof(string));
        var taskIdColumn = new DataColumn("TaskId", typeof(string));
        var conditionColumn = new DataColumn("Condition", typeof(string));
        var planningStartDateColumn = new DataColumn("PlanningStartDate", typeof(DateTime));
        var planningEndDateColumn = new DataColumn("PlanningEndDate", typeof(DateTime));
        var supervisorColumn = new DataColumn("Supervisor", typeof(string));
        var calendarIdColumn = new DataColumn("CalendarId", typeof(string));
        var workOrderStatusColumn = new DataColumn("WorkOrderStatus", typeof(string));
        var suspendColumn = new DataColumn("Suspend", typeof(int));
        var createdByColumn = new DataColumn("CreatedBy", typeof(string));
        var createdDateTimeColumn = new DataColumn("CreatedDateTime", typeof(DateTime));
        var modifiedByColumn = new DataColumn("ModifiedBy", typeof(string));
        var modifiedDateTimeColumn = new DataColumn("ModifiedDateTime", typeof(DateTime));

        dataTable.Columns.Add(workOrderLineIdColumn);
        dataTable.Columns.Add(workOrderHeaderIdColumn);
        dataTable.Columns.Add(lineColumn);
        dataTable.Columns.Add(lineTitleColumn);
        dataTable.Columns.Add(entityIdColumn);
        dataTable.Columns.Add(entityShutdownColumn);
        dataTable.Columns.Add(workOrderTypeColumn);
        dataTable.Columns.Add(taskIdColumn);
        dataTable.Columns.Add(conditionColumn);
        dataTable.Columns.Add(planningStartDateColumn);
        dataTable.Columns.Add(planningEndDateColumn);
        dataTable.Columns.Add(supervisorColumn);
        dataTable.Columns.Add(calendarIdColumn);
        dataTable.Columns.Add(workOrderStatusColumn);
        dataTable.Columns.Add(suspendColumn);
        dataTable.Columns.Add(createdByColumn);
        dataTable.Columns.Add(createdDateTimeColumn);
        dataTable.Columns.Add(modifiedByColumn);
        dataTable.Columns.Add(modifiedDateTimeColumn);

        foreach (var entity in entities)
        {
            onBeforeAdd?.Invoke(this, new AddEventArgs(entity));
            var row = dataTable.NewRow();
            row[workOrderLineIdColumn] = entity.WorkOrderLineId;
            row[workOrderHeaderIdColumn] = entity.WorkOrderHeaderId;
            row[lineColumn] = entity.Line;
            row[lineTitleColumn] = entity.LineTitle;
            row[entityIdColumn] = entity.EntityId;
            row[entityShutdownColumn] = entity.EntityShutdown;
            row[workOrderTypeColumn] = entity.WorkOrderType;
            row[taskIdColumn] = entity.TaskId;
            row[conditionColumn] = entity.Condition;
            row[planningStartDateColumn] = entity.PlanningStartDate;
            row[planningEndDateColumn] = entity.PlanningEndDate;
            row[supervisorColumn] = entity.Supervisor;
            row[calendarIdColumn] = entity.CalendarId;
            row[workOrderStatusColumn] = entity.WorkOrderStatus;
            row[suspendColumn] = entity.Suspend;
            row[createdByColumn] = entity.CreatedBy;
            row[createdDateTimeColumn] = DateTime.Now;
            row[modifiedByColumn] = entity.ModifiedBy;
            row[modifiedDateTimeColumn] = DateTime.Now;
            dataTable.Rows.Add(row);
            onAfterAdd?.Invoke(this, new AddEventArgs(entity));
        }

        var mySqlConnection = _sqlConnection as MySqlConnection ??
                              throw new InvalidOperationException("Connection is not initialized");

        mySqlConnection.InfoMessage += (_, args) =>
        {
            foreach (var message in args.Errors) Console.WriteLine($"Message: {message.Message}");
        };

        var mySqlBulkCopy = new MySqlBulkCopy(mySqlConnection, _dbTransaction as MySqlTransaction)
        {
            DestinationTableName = "WorkOrderLines"
        };

        _ = await mySqlBulkCopy.WriteToServerAsync(dataTable);
    }
}