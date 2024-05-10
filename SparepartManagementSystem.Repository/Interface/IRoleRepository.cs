using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

/// <summary>
/// <para>
///     Role repository interface for <see cref="Role"/> model, inherit from <see cref="IRepository{T}"/>
/// </para>
/// <para>
///     the purpose of this interface is to process the data between the application and the database.
/// </para>
/// <para>
///     use this interface to do CRUD process of the <see cref="Role"/> model.
/// </para>
/// </summary>
public interface IRoleRepository : IRepository<Role>
{
    Task AddUser(int roleId, int userId);
    Task DeleteUser(int roleId, int userId);
    Task<IEnumerable<Role>> GetAllWithUsers();
    Task<Role> GetByIdWithUsers(int id);
}