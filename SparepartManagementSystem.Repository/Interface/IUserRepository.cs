using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

/// <summary>
/// <para>
///     User repository interface for <see cref="User"/> model, inherit from <see cref="IRepository{T}"/>
/// </para>
/// <para>
///     the purpose of this interface is to process the data between the application and the database.
/// </para>
/// <para>
///     use this interface to do CRUD process of the <see cref="User"/> model.
/// </para>
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// <para>
    ///     Get all users with roles
    /// </para>
    /// <para>
    ///     be careful when using this method, it will return all users with roles without pagination, and potentially cause performance issue.
    /// </para>
    /// </summary>
    /// <returns><see cref="IEnumerable{User}"/> where <typeparamref name="T"/> is <see cref="User"/></returns>
    Task<IEnumerable<User>> GetAllWithRoles();

    /// <summary>
    /// Get user by id with roles
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns><see cref="User"/> with multiple <see cref="Role"/> inside</returns>
    Task<User> GetByIdWithRoles(int id);

    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns><see cref="User"/></returns>
    Task<User> GetByUsername(string username);

    /// <summary>
    /// Get user by username with roles
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns><see cref="User"/> with multiple <see cref="Role"/> inside</returns>
    Task<User> GetByUsernameWithRoles(string username);
}