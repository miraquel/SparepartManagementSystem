using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IUserWarehouseRepository : IRepository<UserWarehouse>
{
    // get by userid
    Task<IEnumerable<UserWarehouse>> GetByUserId(int userId, bool forUpdate = false);
    Task<UserWarehouse> GetDefaultByUserId(int userId);
}