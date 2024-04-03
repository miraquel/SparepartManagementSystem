using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IWorkOrderService
{
    public Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto);
    public Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto);
    public Task<ServiceResponse> DeleteWorkOrderHeader(int id);
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderById(int id);
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetAllWorkOrderHeaderPagedList(int pageNumber, int pageSize);
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderByParamsPagedList(int pageNumber, int pageSize, WorkOrderHeaderDto entity);
    public Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse> DeleteWorkOrderLine(int id);
    public Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLineById(int id);
    public Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineByWorkOrderHeaderId(int id);
}