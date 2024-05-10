namespace SparepartManagementSystem.Repository.Interface;

/// <summary>
/// the generics interface for repository layer, use this interface to do CRUD process of the model.
/// </summary>
/// <typeparam name="T">the model of the repository</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Add method to add new record to the database
    /// </summary>
    /// <param name="entity">the model represent as the parameters of the insertion</param>
    /// <returns>returns <typeparamref name="T"/> object</returns>
    Task Add(T entity);

    /// <summary>
    /// Delete method to delete record from the database
    /// </summary>
    /// <param name="id">the id of the record</param>
    /// <returns>returns <typeparamref name="T"/> object</returns>
    Task Delete(int id);

    /// <summary>
    /// Get all records from the database
    /// <para>
    ///     be careful when using this method because it will return all records from the database without any filter, and possibly can lead to performance drop.
    /// </para>
    /// </summary>
    /// <returns>returns <see cref="IEnumerable{T}"/> of <typeparamref name="T"/></returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Get record by id from the database
    /// </summary>
    /// <param name="id">the id of the model</param>
    /// <param name="forUpdate"></param>
    /// <returns>returns <typeparamref name="T"/> object</returns>
    Task<T> GetById(int id, bool forUpdate = false);

    /// <summary>
    /// Get record by parameters from the database
    /// </summary>
    /// <param name="entity">the model represent as the parameters of the selection</param>
    /// <returns>returns <see cref="IEnumerable{T}"/> of <typeparamref name="T"/></returns>
    Task<IEnumerable<T>> GetByParams(T entity);

    /// <summary>
    /// Update method to update record from the database
    /// </summary>
    /// <param name="entity">the model represent as the parameters of the update</param>
    /// <returns>returns <typeparamref name="T"/> object</returns>
    Task Update(T entity);

    DatabaseProvider DatabaseProvider { get; }
}