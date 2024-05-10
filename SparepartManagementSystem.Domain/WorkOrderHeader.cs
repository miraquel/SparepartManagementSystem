using System.Data.SqlTypes;
using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class WorkOrderHeader : BaseModel
{
    public static WorkOrderHeader ForUpdate(WorkOrderHeader oldRecord, WorkOrderHeader newRecord)
    {
        return new WorkOrderHeader
        {
            WorkOrderHeaderId = oldRecord.WorkOrderHeaderId,
            IsSubmitted = oldRecord.IsSubmitted != newRecord.IsSubmitted ? newRecord.IsSubmitted : null,
            SubmittedDate = oldRecord.SubmittedDate != newRecord.SubmittedDate ? newRecord.SubmittedDate : SqlDateTime.MinValue.Value,
            AGSEAMWOID = oldRecord.AGSEAMWOID != newRecord.AGSEAMWOID ? newRecord.AGSEAMWOID : "",
            AGSEAMWRID = oldRecord.AGSEAMWRID != newRecord.AGSEAMWRID ? newRecord.AGSEAMWRID : "",
            AGSEAMEntityID = oldRecord.AGSEAMEntityID != newRecord.AGSEAMEntityID ? newRecord.AGSEAMEntityID : "",
            Name = oldRecord.Name != newRecord.Name ? newRecord.Name : "",
            HeaderTitle = oldRecord.HeaderTitle != newRecord.HeaderTitle ? newRecord.HeaderTitle : "",
            AGSEAMPriorityID = oldRecord.AGSEAMPriorityID != newRecord.AGSEAMPriorityID ? newRecord.AGSEAMPriorityID : "",
            AGSEAMWOTYPE = oldRecord.AGSEAMWOTYPE != newRecord.AGSEAMWOTYPE ? newRecord.AGSEAMWOTYPE : "",
            AGSEAMWOStatusID = oldRecord.AGSEAMWOStatusID != newRecord.AGSEAMWOStatusID ? newRecord.AGSEAMWOStatusID : "",
            AGSEAMPlanningStartDate = oldRecord.AGSEAMPlanningStartDate != newRecord.AGSEAMPlanningStartDate ? newRecord.AGSEAMPlanningStartDate : SqlDateTime.MinValue.Value,
            AGSEAMPlanningEndDate = oldRecord.AGSEAMPlanningEndDate != newRecord.AGSEAMPlanningEndDate ? newRecord.AGSEAMPlanningEndDate : SqlDateTime.MinValue.Value,
            EntityShutDown = oldRecord.EntityShutDown != newRecord.EntityShutDown ? newRecord.EntityShutDown : NoYes.No,
            WOCloseDate = oldRecord.WOCloseDate != newRecord.WOCloseDate ? newRecord.WOCloseDate : SqlDateTime.MinValue.Value,
            AGSEAMSuspend = oldRecord.AGSEAMSuspend != newRecord.AGSEAMSuspend ? newRecord.AGSEAMSuspend : NoYes.No,
            Notes = oldRecord.Notes != newRecord.Notes ? newRecord.Notes : "",
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : SqlDateTime.MinValue.Value,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : SqlDateTime.MinValue.Value
        };
    }

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
    public ICollection<WorkOrderLine> WorkOrderLines { get; set; } = new List<WorkOrderLine>();
}