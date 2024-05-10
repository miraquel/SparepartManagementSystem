using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class WorkOrderLine : BaseModel
{
    public static WorkOrderLine ForUpdate(WorkOrderLine oldRecord, WorkOrderLine newRecord)
    {
        return new WorkOrderLine
        {
            WorkOrderLineId = oldRecord.WorkOrderLineId,
            WorkOrderHeaderId = oldRecord.WorkOrderHeaderId,
            Line = oldRecord.Line != newRecord.Line ? newRecord.Line : 0,
            LineTitle = oldRecord.LineTitle != newRecord.LineTitle ? newRecord.LineTitle : "",
            EntityId = oldRecord.EntityId != newRecord.EntityId ? newRecord.EntityId : "",
            EntityShutdown = oldRecord.EntityShutdown != newRecord.EntityShutdown ? newRecord.EntityShutdown : NoYes.No,
            WorkOrderType = oldRecord.WorkOrderType != newRecord.WorkOrderType ? newRecord.WorkOrderType : "",
            TaskId = oldRecord.TaskId != newRecord.TaskId ? newRecord.TaskId : "",
            Condition = oldRecord.Condition != newRecord.Condition ? newRecord.Condition : "",
            PlanningStartDate = oldRecord.PlanningStartDate != newRecord.PlanningStartDate ? newRecord.PlanningStartDate : DateTime.MinValue,
            PlanningEndDate = oldRecord.PlanningEndDate != newRecord.PlanningEndDate ? newRecord.PlanningEndDate : DateTime.MinValue,
            Supervisor = oldRecord.Supervisor != newRecord.Supervisor ? newRecord.Supervisor : "",
            CalendarId = oldRecord.CalendarId != newRecord.CalendarId ? newRecord.CalendarId : "",
            WorkOrderStatus = oldRecord.WorkOrderStatus != newRecord.WorkOrderStatus ? newRecord.WorkOrderStatus : "",
            Suspend = oldRecord.Suspend != newRecord.Suspend ? newRecord.Suspend : NoYes.No,
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : DateTime.MinValue,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : DateTime.MinValue
        };
    }

    public int WorkOrderLineId { get; set; }
    public int WorkOrderHeaderId { get; set; }
    public int Line { get; set; }
    public string LineTitle { get; set; } = "";
    public string EntityId { get; set; } = "";
    public NoYes EntityShutdown { get; set; }
    public string WorkOrderType { get; set; } = "";
    public string TaskId { get; set; } = "";
    public string Condition { get; set; } = "";
    public DateTime PlanningStartDate { get; set; }
    public DateTime PlanningEndDate { get; set; }
    public string Supervisor { get; set; } = "";
    public string CalendarId { get; set; } = "";
    public string WorkOrderStatus { get; set; } = "";
    public NoYes Suspend { get; set; }
}