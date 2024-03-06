using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain;

public class User : BaseModel
{
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public List<Role> Roles { get; set; } = new();
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public bool? IsAdministrator { get; set; }
    public bool? IsEnabled { get; set; }
    public DateTime LastLogin { get; set; } = SqlDateTime.MinValue.Value;
}