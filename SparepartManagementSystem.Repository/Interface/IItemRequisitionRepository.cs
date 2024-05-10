using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IItemRequisitionRepository : IRepository<ItemRequisition>
{
    public Task<IEnumerable<ItemRequisition>> GetByWorkOrderLineId(int id);
}