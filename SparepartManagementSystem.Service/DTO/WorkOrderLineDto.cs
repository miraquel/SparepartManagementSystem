using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class WorkOrderLineDto
{
    public int WorkOrderLineId { get; init; }
    public int WorkOrderHeaderId { get; init; }
    public int Line { get; init; }
    public string LineTitle { get; init; } = "";
    public string EntityId { get; init; } = "";
    public NoYes EntityShutdown { get; init; }
    public string WorkOrderType { get; init; } = "";
    public string TaskId { get; init; } = "";
    public string Condition { get; init; } = "";
    public DateTime PlanningStartDate { get; init; }
    public DateTime PlanningEndDate { get; init; }
    public string Supervisor { get; init; } = "";
    public string CalendarId { get; init; } = "";
    public string WorkOrderStatus { get; init; } = "";
    public NoYes Suspend { get; init; }
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; }
}