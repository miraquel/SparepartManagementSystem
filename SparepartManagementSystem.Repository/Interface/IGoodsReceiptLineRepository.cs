using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IGoodsReceiptLineRepository : IRepository<GoodsReceiptLine>
{
    Task<IEnumerable<GoodsReceiptLine>> GetByGoodsReceiptHeaderId(int goodsReceiptHeaderId);
    Task BulkAdd(IEnumerable<GoodsReceiptLine> entities);
}