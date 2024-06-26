using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IItemRequisitionRepository
{
    Task Add(ItemRequisition entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(ItemRequisition entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<ItemRequisition>> GetAll();
    Task<ItemRequisition> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<ItemRequisition>> GetByParams(Dictionary<string, string> parameters);
    public Task<IEnumerable<ItemRequisition>> GetByWorkOrderLineId(int id);
    DatabaseProvider DatabaseProvider { get; }
}