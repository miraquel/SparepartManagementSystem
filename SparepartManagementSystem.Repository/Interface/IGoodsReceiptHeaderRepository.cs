using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IGoodsReceiptHeaderRepository : IRepository<GoodsReceiptHeader>
{
    Task<PagedList<GoodsReceiptHeader>> GetAllPagedList(int pageNumber, int pageSize);
    Task<PagedList<GoodsReceiptHeader>> GetByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters);
    Task<GoodsReceiptHeader> GetByIdWithLines(int id, bool forUpdate = false);
}