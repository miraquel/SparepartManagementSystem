using System.Security.Claims;
using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Service.Interface;

public interface ILoginService
{
    bool LoginWithUsernameAndPassword(string username, string password);
    string GenerateToken(User user, bool isRefreshToken = false);
    ClaimsPrincipal ValidateToken(string token);
}