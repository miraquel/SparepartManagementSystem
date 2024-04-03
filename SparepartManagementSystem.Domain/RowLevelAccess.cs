using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class RowLevelAccess : BaseModel
{
    public int RowLevelAccessId { get; set; }
    public int UserId { get; set; }
    public AxTable AxTable { get; set; }
    public string Query { get; set; } = "";
    private ICollection<User> Users { get; init; } = new List<User>();
}