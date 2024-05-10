using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class User : BaseModel
{
    public static User ForUpdate(User oldRecord, User newRecord)
    {
        return new User
        {
            UserId = oldRecord.UserId,
            Username = oldRecord.Username != newRecord.Username ? newRecord.Username : "",
            FirstName = oldRecord.FirstName != newRecord.FirstName ? newRecord.FirstName : "",
            LastName = oldRecord.LastName != newRecord.LastName ? newRecord.LastName : "",
            Email = oldRecord.Email != newRecord.Email ? newRecord.Email : "",
            IsAdministrator = oldRecord.IsAdministrator != newRecord.IsAdministrator ? newRecord.IsAdministrator : null,
            IsEnabled = oldRecord.IsEnabled != newRecord.IsEnabled ? newRecord.IsEnabled : null,
            LastLogin = oldRecord.LastLogin != newRecord.LastLogin ? newRecord.LastLogin : SqlDateTime.MinValue.Value,
            CreatedBy = oldRecord.CreatedBy != newRecord.CreatedBy ? newRecord.CreatedBy : "",
            CreatedDateTime = oldRecord.CreatedDateTime != newRecord.CreatedDateTime ? newRecord.CreatedDateTime : DateTime.MinValue,
            ModifiedBy = oldRecord.ModifiedBy != newRecord.ModifiedBy ? newRecord.ModifiedBy : "",
            ModifiedDateTime = oldRecord.ModifiedDateTime != newRecord.ModifiedDateTime ? newRecord.ModifiedDateTime : DateTime.MinValue
        };
    }
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<UserWarehouse> UserWarehouses { get; set; } = [];
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public bool? IsAdministrator { get; set; }
    public bool? IsEnabled { get; set; }
    public DateTime LastLogin { get; set; } = SqlDateTime.MinValue.Value;
}