using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IWorkOrderHeaderRepository
{
    Task Add(WorkOrderHeader entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(WorkOrderHeader entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<WorkOrderHeader>> GetAll();
    Task<WorkOrderHeader> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<WorkOrderHeader>> GetByParams(Dictionary<string, string> parameters);
    Task<PagedList<WorkOrderHeader>> GetAllPagedList(int pageNumber, int pageSize);
    Task<PagedList<WorkOrderHeader>> GetByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters);
    Task<WorkOrderHeader> GetByIdWithLines(int id);
    DatabaseProvider DatabaseProvider { get; }
}