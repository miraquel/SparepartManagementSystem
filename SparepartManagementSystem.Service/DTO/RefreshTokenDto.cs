using System.Data.SqlTypes;

namespace SparepartManagementSystem.Service.DTO;

public class RefreshTokenDto
{
    public int RefreshTokenId { get; init; }
    public int UserId { get; init; }
    public string Token { get; init; } = "";
    public DateTime Expires { get; init; }
    public DateTime Created { get; init; }
    public DateTime Revoked { get; init; } = SqlDateTime.MinValue.Value;
    public string ReplacedByToken { get; init; } = "";
    public bool IsActive => Revoked == SqlDateTime.MinValue.Value && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;
}