using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IGoodsReceiptHeaderService : IService<GoodsReceiptHeaderDto>
{
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllPagedList(int pageNumber, int pageSize);
    Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> GetByIdWithLines(int id);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> AddWithLines(GoodsReceiptHeaderDto dto);
    Task<ServiceResponse<GoodsReceiptHeaderDto>> PostToAx(GoodsReceiptHeaderDto dto);
}