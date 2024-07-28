using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IGMKSMSServiceGroup
{
    Task<ServiceResponse<InventTableDto>> GetInventTable(string itemId);
    Task<ServiceResponse<PagedListDto<InventTableDto>>> GetInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto);
    Task<ServiceResponse<string>> GetInventTableLabelTemplate(InventTableDto dto, int copies = 1);
    Task<ServiceResponse<PagedListDto<InventTableDto>>> GetRawInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto);
    Task<ServiceResponse<PagedListDto<PurchTableDto>>> GetPurchTablePagedList(int pageNumber, int pageSize, PurchTableSearchDto dto);
    Task<ServiceResponse<IEnumerable<PurchLineDto>>> GetPurchLineList(string purchId);
    Task<ServiceResponse<PagedListDto<WMSLocationDto>>> GetWMSLocationPagedList(int pageNumber, int pageSize, WMSLocationSearchDto dto);
    Task<ServiceResponse<WMSLocationDto>> GetWMSLocation(WMSLocationDto dto);
    Task<ServiceResponse<GMKServiceResponseDto>> PostPurchPackingSlip(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse<PurchTableDto>> GetPurchTable(string purchId);
    Task<ServiceResponse<IEnumerable<InventSumDto>>> GetInventSumList(InventSumSearchDto dto);
    Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderPagedListV2(int pageNumber, int pageSize,
        WorkOrderHeaderDto headerDto);
    Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineListV2(string workOrderHeaderId);
    Task<ServiceResponse<IEnumerable<InventLocationDto>>> GetInventLocationList(InventLocationDto dto);
    Task<ServiceResponse<IEnumerable<DimensionDto>>> GetDimensionList(string dimensionName);
    Task<ServiceResponse<VendPackingSlipJourDto>> GetVendPackingSlipJourWithLines(VendPackingSlipJourDto dto);
    
    #region Obsolete
    [Obsolete("Use GetWorkOrderPagedListV2 instead", true)]
    Task<ServiceResponse<PagedListDto<WorkOrderAxDto>>> GetWorkOrderPagedList(int pageNumber, int pageSize, WorkOrderAxSearchDto dto);
    [Obsolete("Use GetWorkOrderLineListV2 instead", true)]
    Task<ServiceResponse<IEnumerable<WorkOrderLineAxDto>>> GetWorkOrderLineList(string workOrderHeaderId);
    #endregion
}