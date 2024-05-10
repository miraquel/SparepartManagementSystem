using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class RowLevelAccessDto 
{
    public int RowLevelAccessId { get; init; }
    public int UserId { get; init; }
    public AxTable AxTable { get; init; }
    public string Query { get; init; } = "";
    public string CreatedBy { get; init; } = "";
    public DateTime CreatedDateTime { get; init; }
    public string ModifiedBy { get; init; } = ""; 
    public DateTime ModifiedDateTime { get; init; }
}