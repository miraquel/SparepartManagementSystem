using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IGoodsReceiptLineRepository : IRepository<GoodsReceiptLine>
{
    Task<IEnumerable<GoodsReceiptLine>> GetByGoodsReceiptHeaderId(int goodsReceiptHeaderId);
    Task<int> BulkAdd(IEnumerable<GoodsReceiptLine> entities);
}