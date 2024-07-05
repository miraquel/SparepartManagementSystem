using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IWorkOrderServiceDirect
{
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeader(string agsEamWoId);
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderPagedList(int pageNumber,
        int pageSize, WorkOrderHeaderDto dto);
    public Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto);
    public Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLine(string agsEamWoId, int line);
    public Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineList(string agsEamWoId);
    public Task<ServiceResponse> CloseWorkOrderLineAndPostInventJournal(WorkOrderLineDto dto);
    public Task<ServiceResponse> AddItemRequisition(InventReqDto dto);
    public Task<ServiceResponse> UpdateItemRequisition(InventReqDto dto);
    public Task<ServiceResponse> DeleteItemRequisition(InventReqDto dto);
    public Task<ServiceResponse> DeleteItemRequisitionWithListOfRecId(IEnumerable<long> recIds);
    public Task<ServiceResponse<InventReqDto>> GetItemRequisition(InventReqDto dto);
    public Task<ServiceResponse<IEnumerable<InventReqDto>>> GetItemRequisitionList(long agsWoRecId);
    public Task<ServiceResponse> CreateInventJournalTable(WorkOrderLineDto dto);
}