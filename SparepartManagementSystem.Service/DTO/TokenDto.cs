using System.ComponentModel;

namespace SparepartManagementSystem.Service.DTO
{
    public class TokenDto
    {
        [DefaultValue("")]
        public string AccessToken { get; init; } = string.Empty;
        [DefaultValue("")]
        public string RefreshToken { get; init; } = string.Empty;
    }
}
