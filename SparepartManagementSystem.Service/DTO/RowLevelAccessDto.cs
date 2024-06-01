using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class RowLevelAccessDto 
{
    public int RowLevelAccessId { get; init; }
    public int UserId { get; init; }
    public AxTable AxTable { get; init; }
    public string Query { get; init; } = string.Empty;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = string.Empty; 
    public DateTime ModifiedDateTime { get; init; }
}