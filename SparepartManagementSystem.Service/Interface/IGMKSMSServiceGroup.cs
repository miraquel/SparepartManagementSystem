using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IGMKSMSServiceGroup
{
    Task<ServiceResponse<PagedListDto<InventTableDto>>> GetInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto);

    Task<ServiceResponse<PagedListDto<PurchTableDto>>> GetPurchTablePagedList(int pageNumber, int pageSize, PurchTableSearchDto dto);
    Task<ServiceResponse<IEnumerable<PurchLineDto>>> GetPurchLineList(string purchId);
    Task<ServiceResponse<PagedListDto<WMSLocationDto>>> GetWMSLocationPagedList(int pageNumber, int pageSize, WMSLocationSearchDto dto);
    Task<ServiceResponse<GMKServiceResponseDto>> PostPurchPackingSlip(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse<PurchTableDto>> GetPurchTable(string purchId);
    Task<ServiceResponse<IEnumerable<InventSumDto>>> GetInventSumList(InventSumSearchDto dto);
    Task<ServiceResponse<PagedListDto<WorkOrderDto>>> GetWorkOrderPagedList(int pageNumber, int pageSize, WorkOrderSearchDto dto);
}