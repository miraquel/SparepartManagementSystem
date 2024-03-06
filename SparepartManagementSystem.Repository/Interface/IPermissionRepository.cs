using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

/// <summary>
/// <para>
///     Permission repository interface for <see cref="Permission"/> model, inherit from <see cref="IRepository{T}"/>
/// </para>
/// <para>
///     the purpose of this interface is to process the data between the application and the database.
/// </para>
/// <para>
///     use this interface to do CRUD process of the <see cref="Permission"/> model.
/// </para>
/// </summary>
public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Get permission by role id
    /// </summary>
    /// <param name="roleId">Role id</param>
    /// <returns><see cref="IEnumerable{Permission}"/> where <typeparamref name="T"/> is <see cref="Permission"/></returns>
    Task<IEnumerable<Permission>> GetByRoleId(int roleId);
}