using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IGoodsReceiptService : IService<GoodsReceiptHeaderDto>
{
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllPagedList(int pageNumber, int pageSize);
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> GetByIdWithLines(int id);
    Task<ServiceResponse> AddWithLines(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse> UpdateWithLines(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse> PostToAx(GoodsReceiptHeaderDto dto);
}