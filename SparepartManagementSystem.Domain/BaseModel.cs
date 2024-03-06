using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public abstract class BaseModel
{
    public DateTime CreatedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    public DateTime ModifiedDateTime { get; set; } = SqlDateTime.MinValue.Value;
    public string CreatedBy { get; set; } = string.Empty;
    public string ModifiedBy { get; set; } = string.Empty;
}