using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.UnitOfWork;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRoleRepository RoleRepository { get; }
    IUserRepository UserRepository { get; }
    IUserWarehouseRepository UserWarehouseRepository { get; }
    IPermissionRepository PermissionRepository { get; }
    INumberSequenceRepository NumberSequenceRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IGoodsReceiptHeaderRepository GoodsReceiptHeaderRepository { get; }
    IGoodsReceiptLineRepository GoodsReceiptLineRepository { get; }
    IRowLevelAccessRepository RowLevelAccessRepository { get; }
    IWorkOrderHeaderRepository WorkOrderHeaderRepository { get; }
    IWorkOrderLineRepository WorkOrderLineRepository { get; }
    IItemRequisitionRepository ItemRequisitionRepository { get; }
    IVersionTrackerRepository VersionTrackerRepository { get; }
    Task<int> GetLastInsertedId();
    Task Commit();
    Task Rollback();
}