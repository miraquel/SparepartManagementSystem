using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class RowLevelAccessDto 
{
    public int RowLevelAccessId { get; set; }
    public int UserId { get; set; }
    public AxTable AxTable { get; set; }
    public string Query { get; set; } = "";
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedDateTime { get; set; }
    public string ModifiedBy { get; set; } = ""; 
    public DateTime ModifiedDateTime { get; set; }
}