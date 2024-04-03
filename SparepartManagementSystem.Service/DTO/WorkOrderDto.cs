using System.Data.SqlClient;
using System.Data.SqlTypes;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.DTO;

public class WorkOrderDto
{
    public string AGSEAMWOID { get; init; } = "";
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
}