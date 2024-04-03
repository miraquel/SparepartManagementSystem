using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IWorkOrderLineRepository : IRepository<WorkOrderLine>
{
    public Task<IEnumerable<WorkOrderLine>> GetByWorkOrderHeaderId(int id);
}