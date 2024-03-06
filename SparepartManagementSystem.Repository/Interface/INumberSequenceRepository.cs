using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface
{
    /// <summary>
    /// <para>
    ///     Number sequence repository interface for <see cref="NumberSequence"/> model, inherit from <see cref="IRepository{T}"/>
    /// </para>
    /// <para>
    ///     the purpose of this interface is to process the data between the application and the database.
    /// </para>
    /// <para>
    ///     use this interface to do CRUD process of the <see cref="NumberSequence"/> model.
    /// </para>
    /// </summary>
    public interface INumberSequenceRepository : IRepository<NumberSequence>
    {
        /// <summary>
        /// Get next number by module
        /// </summary>
        /// <param name="module">Module name</param>
        /// <returns><see cref="int"/> as the next number</returns>
        Task<string> GetNextNumberByModule(string module);
    }
}
