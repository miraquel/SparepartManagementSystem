using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class WorkOrderHeaderDto
{
    public int WorkOrderHeaderId { get; set; }
    public bool? IsSubmitted { get; set; }
    public DateTime SubmittedDate { get; set; } = SqlDateTime.MinValue.Value;
    public string AGSEAMWOID { get; set; } = "";
    public string AGSEAMWRID { get; init; } = "";
    public string AGSEAMEntityID { get; init; } = "";
    public string Name { get; init; } = "";
    public string HeaderTitle { get; init; } = "";
    public string AGSEAMPriorityID { get; init; } = "";
    public string AGSEAMWOTYPE { get; init; } = "";
    public string AGSEAMWOStatusID { get; init; } = "";
    public DateTime AGSEAMPlanningStartDate { get; init; } = SqlDateTime.MinValue.Value;
    public DateTime AGSEAMPlanningEndDate { get; init; } = SqlDateTime.MinValue.Value;
    public NoYes EntityShutDown { get; init; }
    public DateTime WOCloseDate { get; init; } = SqlDateTime.MinValue.Value;
    public NoYes AGSEAMSuspend { get; init; }
    public string Notes { get; init; } = "";
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public string ModifiedBy { get; init; } = "";
    public DateTime ModifiedDateTime { get; init; } = SqlDateTime.MinValue.Value;
    public ICollection<WorkOrderLineDto> WorkOrderLines { get; set; } = new List<WorkOrderLineDto>();
}