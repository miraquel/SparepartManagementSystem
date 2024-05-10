using System.Data;
using System.Data.SqlTypes;
using Dapper;
using MySqlConnector;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.Interface;

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

    public async Task Add(WorkOrderLine entity)
    {
        const string sql = """
                           INSERT INTO WorkOrderLines
                               (WorkOrderHeaderId, Line, LineTitle, EntityId, EntityShutdown, WorkOrderType, TaskId, "Condition", PlanningStartDate, PlanningEndDate, Supervisor, CalendarId, WorkOrderStatus, Suspend, CreatedBy, CreatedDateTime, ModifiedBy, ModifiedDateTime)
                           VALUES
                               (@WorkOrderHeaderId, @Line, @LineTitle, @EntityId, @EntityShutdown, @WorkOrderType, @TaskId, @Condition, @PlanningStartDate, @PlanningEndDate, @Supervisor, @CalendarId, @WorkOrderStatus, @Suspend, @CreatedBy, @CreatedDateTime, @ModifiedBy, @ModifiedDateTime);
                           """;
        
        _ = await _sqlConnection.ExecuteAsync(sql, entity, _dbTransaction);
    }

    public async Task Delete(int id)
    {
        const string sql = "DELETE FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId";
        _ = await _sqlConnection.ExecuteAsync(sql, new { WorkOrderLineId = id }, _dbTransaction);
    }

    public Task<IEnumerable<WorkOrderLine>> GetAll()
    {
        const string sql = "SELECT * FROM WorkOrderLines";
        return _sqlConnection.QueryAsync<WorkOrderLine>(sql, transaction: _dbTransaction);
    }

    public Task<WorkOrderLine> GetById(int id, bool forUpdate = false)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId";
        const string sqlForUpdate = "SELECT * FROM WorkOrderLines WHERE WorkOrderLineId = @WorkOrderLineId FOR UPDATE";
        return _sqlConnection.QueryFirstAsync<WorkOrderLine>(forUpdate ? sqlForUpdate : sql, new { WorkOrderLineId = id }, _dbTransaction);
    }

    public Task<IEnumerable<WorkOrderLine>> GetByParams(WorkOrderLine entity)
    {
        var sqlBuilder = new SqlBuilder();

        if (entity.WorkOrderHeaderId != 0)
        {
            sqlBuilder.Where("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        }

        if (entity.Line != 0)
        {
            sqlBuilder.Where("Line = @Line", new { entity.Line });
        }

        if (!string.IsNullOrEmpty(entity.LineTitle))
        {
            sqlBuilder.Where("LineTitle LIKE @LineTitle", new { LineTitle = $"%{entity.LineTitle}%" });
        }

        if (!string.IsNullOrEmpty(entity.EntityId))
        {
            sqlBuilder.Where("EntityId LIKE @EntityId", new { EntityId = $"%{entity.EntityId}%" });
        }

        if (entity.EntityShutdown is not NoYes.None)
        {
            sqlBuilder.Where("EntityShutdown = @EntityShutdown", new { entity.EntityShutdown });
        }

        if (!string.IsNullOrEmpty(entity.WorkOrderType))
        {
            sqlBuilder.Where("WorkOrderType LIKE @WorkOrderType", new { WorkOrderType = $"%{entity.WorkOrderType}%" });
        }

        if (!string.IsNullOrEmpty(entity.TaskId))
        {
            sqlBuilder.Where("TaskId LIKE @TaskId", new { TaskId = $"%{entity.TaskId}%" });
        }

        if (!string.IsNullOrEmpty(entity.Condition))
        {
            sqlBuilder.Where("Condition LIKE @Condition", new { Condition = $"%{entity.Condition}%" });
        }

        if (entity.PlanningStartDate > SqlDateTime.MinValue.Value)
        {
            sqlBuilder.Where("PlanningStartDate = @PlanningStartDate", new { entity.PlanningStartDate });
        }

        if (entity.PlanningEndDate > SqlDateTime.MinValue.Value)
        {
            sqlBuilder.Where("PlanningEndDate = @PlanningEndDate", new { entity.PlanningEndDate });
        }

        if (!string.IsNullOrEmpty(entity.Supervisor))
        {
            sqlBuilder.Where("Supervisor LIKE @Supervisor", new { Supervisor = $"%{entity.Supervisor}%" });
        }

        if (!string.IsNullOrEmpty(entity.CalendarId))
        {
            sqlBuilder.Where("CalendarId LIKE @CalendarId", new { CalendarId = $"%{entity.CalendarId}%" });
        }

        if (!string.IsNullOrEmpty(entity.WorkOrderStatus))
        {
            sqlBuilder.Where("WorkOrderStatus LIKE @WorkOrderStatus", new { WorkOrderStatus = $"%{entity.WorkOrderStatus}%" });
        }

        if (entity.Suspend is not NoYes.None)
        {
            sqlBuilder.Where("Suspend = @Suspend", new { entity.Suspend });
        }

        const string sql = "SELECT * FROM WorkOrderLines /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        
        return _sqlConnection.QueryAsync<WorkOrderLine>(template.RawSql, template.Parameters, _dbTransaction);
    }

    public async Task Update(WorkOrderLine entity)
    {
        var sqlBuilder = new SqlBuilder();
        
        if (entity.WorkOrderHeaderId != 0)
            sqlBuilder.Set("WorkOrderHeaderId = @WorkOrderHeaderId", new { entity.WorkOrderHeaderId });
        if (entity.Line != 0)
            sqlBuilder.Set("Line = @Line", new { entity.Line });
        if (!string.IsNullOrEmpty(entity.LineTitle))
            sqlBuilder.Set("LineTitle = @LineTitle", new { entity.LineTitle });
        if (!string.IsNullOrEmpty(entity.EntityId))
            sqlBuilder.Set("EntityId = @EntityId", new { entity.EntityId });
        if (entity.EntityShutdown != NoYes.None)
            sqlBuilder.Set("EntityShutdown = @EntityShutdown", new { entity.EntityShutdown });
        if (!string.IsNullOrEmpty(entity.WorkOrderType))
            sqlBuilder.Set("WorkOrderType = @WorkOrderType", new { entity.WorkOrderType });
        if (!string.IsNullOrEmpty(entity.TaskId))
            sqlBuilder.Set("TaskId = @TaskId", new { entity.TaskId });
        if (!string.IsNullOrEmpty(entity.Condition))
            sqlBuilder.Set("Condition = @Condition", new { entity.Condition });
        if (entity.PlanningStartDate != SqlDateTime.MinValue.Value)
            sqlBuilder.Set("PlanningStartDate = @PlanningStartDate", new { entity.PlanningStartDate });
        if (entity.PlanningEndDate != SqlDateTime.MinValue.Value)
            sqlBuilder.Set("PlanningEndDate = @PlanningEndDate", new { entity.PlanningEndDate });
        if (!string.IsNullOrEmpty(entity.Supervisor))
            sqlBuilder.Set("Supervisor = @Supervisor", new { entity.Supervisor });
        if (!string.IsNullOrEmpty(entity.CalendarId))
            sqlBuilder.Set("CalendarId = @CalendarId", new { entity.CalendarId });
        if (!string.IsNullOrEmpty(entity.WorkOrderStatus))
            sqlBuilder.Set("WorkOrderStatus = @WorkOrderStatus", new { entity.WorkOrderStatus });
        if (entity.Suspend != NoYes.None)
            sqlBuilder.Set("Suspend = @Suspend", new { entity.Suspend });
        if (!string.IsNullOrEmpty(entity.ModifiedBy))
            sqlBuilder.Set("ModifiedBy = @ModifiedBy", new { entity.ModifiedBy });
        if (entity.ModifiedDateTime != DateTime.MinValue)
            sqlBuilder.Set("ModifiedDateTime = @ModifiedDateTime", new { entity.ModifiedDateTime });
        
        sqlBuilder.Where("WorkOrderLineId = @WorkOrderLineId", new { entity.WorkOrderLineId });

        const string sql = "UPDATE WorkOrderLines /**set**/ /**where**/";
        var template = sqlBuilder.AddTemplate(sql);
        
        _ = await _sqlConnection.ExecuteAsync(template.RawSql, template.Parameters, _dbTransaction);
    }

    public Task<int> GetLastInsertedId()
    {
        const string sql = "SELECT LAST_INSERT_ID()";
        return _sqlConnection.QueryFirstAsync<int>(sql, transaction: _dbTransaction);
    }

    public DatabaseProvider DatabaseProvider => DatabaseProvider.MySql;
    public Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id)
    {
        const string sql = "SELECT * FROM WorkOrderLines WHERE WorkOrderHeaderId = @WorkOrderHeaderId";
        return _sqlConnection.QueryAsync<WorkOrderLine>(sql, new { WorkOrderHeaderId = id }, _dbTransaction);
    }

    public async Task BulkAdd(IEnumerable<WorkOrderLine> entities)
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
            DestinationTableName = "WorkOrderLines"
        };

        _ = await mySqlBulkCopy.WriteToServerAsync(dataTable);
    }
}