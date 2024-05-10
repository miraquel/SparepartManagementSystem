using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IGoodsReceiptService
{
    Task<ServiceResponse> AddGoodsReceiptHeader(GoodsReceiptHeaderDto entity);
    Task<ServiceResponse> UpdateGoodsReceiptHeader(GoodsReceiptHeaderDto entity);
    Task<ServiceResponse> DeleteGoodsReceiptHeader(int id);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> GetGoodsReceiptHeaderById(int id);
    Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeader();
    Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetGoodsReceiptHeaderByParams(GoodsReceiptHeaderDto entity);
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeaderPagedList(int pageNumber, int pageSize);
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> GetGoodsReceiptHeaderByIdWithLines(int id);
    Task<ServiceResponse> AddGoodsReceiptHeaderWithLines(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse> UpdateGoodsReceiptHeaderWithLines(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse> PostToAx(GoodsReceiptHeaderDto dto);
}