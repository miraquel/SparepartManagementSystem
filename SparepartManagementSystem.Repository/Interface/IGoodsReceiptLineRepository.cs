using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IGoodsReceiptLineRepository
{
    Task Add(GoodsReceiptLine entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Update(GoodsReceiptLine entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null);
    Task Delete(int id);
    Task<IEnumerable<GoodsReceiptLine>> GetAll();
    Task<GoodsReceiptLine> GetById(int id, bool forUpdate = false);
    Task<GoodsReceiptHeader> GetByIdWithGoodsReceiptHeader(int id, bool forUpdate = false);
    Task<IEnumerable<GoodsReceiptLine>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<GoodsReceiptLine>> GetByGoodsReceiptHeaderId(int goodsReceiptHeaderId);
    Task BulkAdd(IEnumerable<GoodsReceiptLine> entities, Action<object, object>? onSqlBulkCopyError = null,
        EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    DatabaseProvider DatabaseProvider { get; }
}