using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IGoodsReceiptHeaderRepository
{
    DatabaseProvider DatabaseProvider { get; }
    Task Add(GoodsReceiptHeader entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Update(GoodsReceiptHeader entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task Delete(int id);
    Task<IEnumerable<GoodsReceiptHeader>> GetAll();
    Task<GoodsReceiptHeader> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<GoodsReceiptHeader>> GetByParams(Dictionary<string, string> parameters);
    Task<PagedList<GoodsReceiptHeader>> GetAllPagedList(int pageNumber, int pageSize);
    Task<PagedList<GoodsReceiptHeader>> GetByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters);
    Task<GoodsReceiptHeader> GetByIdWithLines(int id, bool forUpdate = false);
}