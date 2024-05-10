using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IWorkOrderHeaderRepository : IRepository<WorkOrderHeader>
{
    public Task<PagedList<WorkOrderHeader>> GetAllPagedList(int pageNumber, int pageSize);
    public Task<PagedList<WorkOrderHeader>> GetByParamsPagedList(int pageNumber, int pageSize, WorkOrderHeader entity);
    public Task<WorkOrderHeader> GetByIdWithLines(int id);
}