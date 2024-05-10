using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Domain;

public class RowLevelAccess : BaseModel
{
    public static RowLevelAccess ForUpdate(RowLevelAccess oldRecord, RowLevelAccess newRecord)
    {
        return new RowLevelAccess
        {
            RowLevelAccessId = oldRecord.RowLevelAccessId,
            UserId = oldRecord.UserId != newRecord.UserId ? newRecord.UserId : 0,
            AxTable = oldRecord.AxTable != newRecord.AxTable ? newRecord.AxTable : new AxTable(),
            Query = oldRecord.Query != newRecord.Query ? newRecord.Query : ""
        };
    }
    public int RowLevelAccessId { get; set; }
    public int UserId { get; set; }
    public AxTable AxTable { get; set; }
    public string Query { get; set; } = "";
}