using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IWorkOrderLineRepository : IRepository<WorkOrderLine>
{
    Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id);
    Task BulkAdd(IEnumerable<WorkOrderLine> entities);
}