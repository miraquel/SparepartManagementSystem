using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IWorkOrderService
{
    public Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto);
    public Task<ServiceResponse> AddWorkOrderHeaderWithLines(WorkOrderHeaderDto dto);
    public Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto);
    public Task<ServiceResponse> DeleteWorkOrderHeader(int id);
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderById(int id);
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetAllWorkOrderHeaderPagedList(int pageNumber, int pageSize);
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderByParamsPagedList(int pageNumber, int pageSize, WorkOrderHeaderDto entity);
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderByIdWithLines(int id);
    public Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse> DeleteWorkOrderLine(int id);
    public Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLineById(int id);
    public Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineByWorkOrderHeaderId(int id);
    public Task<ServiceResponse> AddItemRequisition(ItemRequisitionDto dto);
    public Task<ServiceResponse> UpdateItemRequisition(ItemRequisitionDto dto);
    public Task<ServiceResponse> DeleteItemRequisition(int id);
    public Task<ServiceResponse<ItemRequisitionDto>> GetItemRequisitionById(int id);
    public Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> GetItemRequisitionByParams(ItemRequisitionDto entity);
    public Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> GetItemRequisitionByWorkOrderLineId(int id);
}