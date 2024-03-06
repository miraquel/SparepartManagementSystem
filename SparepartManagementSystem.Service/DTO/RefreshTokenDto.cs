using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class RefreshTokenDto
{
    public int RefreshTokenId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = "";
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime Revoked { get; set; } = SqlDateTime.MinValue.Value;
    public string ReplacedByToken { get; set; } = "";
    public bool IsActive => Revoked == SqlDateTime.MinValue.Value && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;
}