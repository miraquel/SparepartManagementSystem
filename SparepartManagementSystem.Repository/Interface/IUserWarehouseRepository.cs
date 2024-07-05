using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IUserWarehouseRepository
{
    Task Add(UserWarehouse entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(UserWarehouse entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<UserWarehouse>> GetAll();
    Task<UserWarehouse> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<UserWarehouse>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<UserWarehouse>> GetByUserId(int userId, bool forUpdate = false);
    Task<UserWarehouse> GetDefaultByUserId(int userId);
    DatabaseProvider DatabaseProvider { get; }
}