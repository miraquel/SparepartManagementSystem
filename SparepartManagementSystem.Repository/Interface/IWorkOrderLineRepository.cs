using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IWorkOrderLineRepository
{
    Task Add(WorkOrderLine entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(WorkOrderLine entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null, EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<WorkOrderLine>> GetAll();
    Task<WorkOrderLine> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<WorkOrderLine>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id);
    Task BulkAdd(IEnumerable<WorkOrderLine> entities, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    DatabaseProvider DatabaseProvider { get; }
}