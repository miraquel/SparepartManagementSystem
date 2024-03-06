using System.ComponentModel;

namespace SparepartManagementSystem.Service.DTO
{
    public class TokenDto
    {
        [DefaultValue("")]
        public string AccessToken { get; set; } = "";
        [DefaultValue("")]
        public string RefreshToken { get; set; } = "";
    }
}
