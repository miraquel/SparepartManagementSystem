using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IRefreshTokenRepository
{
    Task Add(RefreshToken entity);
    Task Delete(int id);
    Task Update(RefreshToken entity);
    Task<IEnumerable<RefreshToken>> GetAll();
    Task<RefreshToken> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<RefreshToken>> GetByParams(Dictionary<string, string> parameters);
    Task<RefreshToken> GetByUserIdAndToken(int userId, string token, bool forUpdate = false);
    Task<IEnumerable<RefreshToken>> GetByUserId(int userId);
    Task Revoke(int id);
    Task RevokeAll(int userId);
    DatabaseProvider DatabaseProvider { get; }
}