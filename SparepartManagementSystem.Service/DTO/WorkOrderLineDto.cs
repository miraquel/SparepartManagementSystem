using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class WorkOrderLineDto
{
    public int WorkOrderLineId { get; init; }
    public int WorkOrderHeaderId { get; init; }
    public int Line { get; init; }
    public string WOID { get; init; } = string.Empty;
    public string LineTitle { get; init; } = string.Empty;
    public string EntityId { get; init; } = string.Empty;
    public NoYes EntityShutdown { get; init; }
    public string WorkOrderType { get; init; } = string.Empty;
    public string TaskId { get; init; } = string.Empty;
    public string Condition { get; init; } = string.Empty;
    public DateTime PlanningStartDate { get; init; }
    public DateTime PlanningEndDate { get; init; }
    public string Supervisor { get; init; } = string.Empty;
    public string CalendarId { get; init; } = string.Empty;
    public string LineStatus { get; init; } = string.Empty;
    public NoYes Suspend { get; init; }
    public DefaultDimensionDto DefaultDimension { get; init; } = new();
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; }
    public long RecId { get; init; }
}