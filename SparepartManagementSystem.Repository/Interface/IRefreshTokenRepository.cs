using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

/// <summary>
/// <para>
///     Refresh token repository interface for <see cref="RefreshToken"/> model, inherit from <see cref="IRepository{T}"/>
/// </para>
/// <para>
///     the purpose of this interface is to process the data between the application and the database.
/// </para>
/// <para>
///     use this interface to do CRUD process of the <see cref="RefreshToken"/> model.
/// </para>
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>
    /// Get refresh token by user id and token
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="token">Token</param>
    /// <param name="forUpdate"></param>
    /// <returns><see cref="RefreshToken"/></returns>
    Task<RefreshToken> GetByUserIdAndToken(int userId, string token, bool forUpdate = false);

    /// <summary>
    /// Get refresh token by user id
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns><see cref="RefreshToken"/></returns>
    Task<IEnumerable<RefreshToken>> GetByUserId(int userId);

    /// <summary>
    /// Revoke refresh token by id
    /// </summary>
    /// <param name="id">Refresh Token Id</param>
    /// <returns><see cref="RefreshToken"/></returns>
    Task Revoke(int id);

    /// <summary>
    /// Revoke all refresh token by user id
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <returns><see cref="RefreshToken"/></returns>
    Task RevokeAll(int userId);
}