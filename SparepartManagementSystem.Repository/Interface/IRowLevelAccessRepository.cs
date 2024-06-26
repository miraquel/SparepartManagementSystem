using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IRowLevelAccessRepository
{
    Task Add(RowLevelAccess entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(RowLevelAccess entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<RowLevelAccess>> GetAll();
    Task<RowLevelAccess> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<RowLevelAccess>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<RowLevelAccess>> GetByUserId(int userId);
    DatabaseProvider DatabaseProvider { get; }
}