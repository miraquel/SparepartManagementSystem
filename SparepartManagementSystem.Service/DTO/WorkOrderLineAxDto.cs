using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

[Obsolete("Use WorkOrderLineDto instead", true)]
public class WorkOrderLineAxDto
{
    public int Line { get; init; }
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
    public string WorkOrderStatus { get; init; } = string.Empty;
    public NoYes Suspend { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public long RecId { get; init; }
}