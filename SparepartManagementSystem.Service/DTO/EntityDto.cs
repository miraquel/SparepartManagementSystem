using System.Data.SqlTypes;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.DTO;

public class EntityDto
{
    public string AGSEAMEntityID { get; init; } = string.Empty;
    public string AGSEAMEntityName { get; init; } = string.Empty;
    public string AGSEAMEntityClass { get; init; } = string.Empty;
    public AGSEAMHierarchyType AGSEAMHierarchyType { get; init; }
    public string AGSEAMEntityIdParent { get; init; } = string.Empty;
    public string AGSEAMLocation { get; init; } = string.Empty;
    public string AGSEAMSiteName { get; init; } = string.Empty;
    public AGSEAMEntityStatus AGSEAMEntityStatus { get; init; }
    public DateTime AGSEAMCommDate { get; init; } = SqlDateTime.MinValue.Value;
    public string AGSEAMCriticalityID { get; init; } = string.Empty;
    public string AGSEAMPriorityID { get; init; } = string.Empty;
}