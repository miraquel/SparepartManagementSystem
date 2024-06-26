using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.Factory;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IEnumerable<IUserRepository> _userRepositories;
    private readonly IEnumerable<IUserWarehouseRepository> _userWarehouseRepositories;
    private readonly IEnumerable<IRoleRepository> _roleRepositories;
    private readonly IEnumerable<INumberSequenceRepository> _numberSequenceRepositories;
    private readonly IEnumerable<IPermissionRepository> _permissionRepositories;
    private readonly IEnumerable<IRefreshTokenRepository> _refreshTokenRepositories;
    private readonly IEnumerable<IGoodsReceiptHeaderRepository> _goodsReceiptHeaderRepositories;
    private readonly IEnumerable<IGoodsReceiptLineRepository> _goodsReceiptLineRepositories;
    private readonly IEnumerable<IRowLevelAccessRepository> _rowLevelAccessRepositories;
    private readonly IEnumerable<IWorkOrderHeaderRepository> _workOrderHeaderRepositories;
    private readonly IEnumerable<IWorkOrderLineRepository> _workOrderLineRepositories;
    private readonly IEnumerable<IItemRequisitionRepository> _itemRequisitionRepositories;
    private readonly IEnumerable<IVersionTrackerRepository> _versionTrackerRepositories;
    public RepositoryFactory(IEnumerable<IUserRepository> userRepositories, IEnumerable<IRoleRepository> roleRepositories, IEnumerable<INumberSequenceRepository> numberSequenceRepositories, IEnumerable<IPermissionRepository> permissionRepositories, IEnumerable<IRefreshTokenRepository> refreshTokenRepositories, IEnumerable<IGoodsReceiptHeaderRepository> goodsReceiptHeaderRepositories, IEnumerable<IGoodsReceiptLineRepository> goodsReceiptLineRepositories, IEnumerable<IRowLevelAccessRepository> rowLevelAccessRepositories, IEnumerable<IWorkOrderHeaderRepository> workOrderHeaderRepositories, IEnumerable<IItemRequisitionRepository> itemRequisitionRepositories, IEnumerable<IWorkOrderLineRepository> workOrderLineRepositories, IEnumerable<IUserWarehouseRepository> userWarehouseRepositories, IEnumerable<IVersionTrackerRepository> versionTrackerRepositories)
    {
        _userRepositories = userRepositories;
        _roleRepositories = roleRepositories;
        _numberSequenceRepositories = numberSequenceRepositories;
        _permissionRepositories = permissionRepositories;
        _refreshTokenRepositories = refreshTokenRepositories;
        _goodsReceiptHeaderRepositories = goodsReceiptHeaderRepositories;
        _goodsReceiptLineRepositories = goodsReceiptLineRepositories;
        _rowLevelAccessRepositories = rowLevelAccessRepositories;
        _workOrderHeaderRepositories = workOrderHeaderRepositories;
        _workOrderLineRepositories = workOrderLineRepositories;
        _userWarehouseRepositories = userWarehouseRepositories;
        _versionTrackerRepositories = versionTrackerRepositories;
        _itemRequisitionRepositories = itemRequisitionRepositories;
    }

    public T GetRepository<T>(DatabaseProvider databaseProvider) where T : class
    {
        return typeof(T) switch
        {
            var type when type == typeof(IUserRepository) => _userRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IUserWarehouseRepository) => _userWarehouseRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IRoleRepository) => _roleRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(INumberSequenceRepository) => _numberSequenceRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IPermissionRepository) => _permissionRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IRefreshTokenRepository) => _refreshTokenRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IGoodsReceiptHeaderRepository) => _goodsReceiptHeaderRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IGoodsReceiptLineRepository) => _goodsReceiptLineRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IRowLevelAccessRepository) => _rowLevelAccessRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IWorkOrderHeaderRepository) => _workOrderHeaderRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IWorkOrderLineRepository) => _workOrderLineRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IItemRequisitionRepository) => _itemRequisitionRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IVersionTrackerRepository) => _versionTrackerRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            _ => throw new ArgumentOutOfRangeException(nameof(T), typeof(T), null)
        } ?? throw new NullReferenceException();
    }

}