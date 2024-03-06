namespace SparepartManagementSystem.Repository.Factory;

public interface IRepositoryFactory
{
    // get repository by interface
    T? GetRepository<T>(DatabaseProvider databaseProvider) where T : class;
}