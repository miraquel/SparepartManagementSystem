using System.Data.SqlTypes;

namespace SparepartManagementSystem.Domain
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public DateTime Expires { get; set; } = SqlDateTime.MinValue.Value;
        public DateTime Created { get; set; } = SqlDateTime.MinValue.Value;
        public DateTime Revoked { get; set; } = SqlDateTime.MinValue.Value;
        public string ReplacedByToken { get; set; } = "";
        public bool IsActive => Revoked == SqlDateTime.MinValue.Value;
        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}