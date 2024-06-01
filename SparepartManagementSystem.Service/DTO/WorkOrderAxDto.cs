using System.Data.SqlTypes;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.DTO;

[Obsolete("Use WorkOrderHeaderDto instead", true)]
public class WorkOrderAxDto
{
    public string AGSEAMWOID { get; init; } = string.Empty;
    public string AGSEAMWRID { get; init; } = string.Empty;
    public string AGSEAMEntityID { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string HeaderTitle { get; init; } = string.Empty;
    public string AGSEAMPriorityID { get; init; } = string.Empty;
    public string AGSEAMWOTYPE { get; init; } = string.Empty;
    public string AGSEAMWOStatusID { get; init; } = string.Empty;
    public DateTime AGSEAMPlanningStartDate { get; init; } = SqlDateTime.MinValue.Value;
    public DateTime AGSEAMPlanningEndDate { get; init; } = SqlDateTime.MinValue.Value;
    public NoYes EntityShutDown { get; init; }
    public DateTime WOCloseDate { get; init; } = SqlDateTime.MinValue.Value;
    public NoYes AGSEAMSuspend { get; init; }
    public string Notes { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = string.Empty;
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public long RecId { get; init; }
}