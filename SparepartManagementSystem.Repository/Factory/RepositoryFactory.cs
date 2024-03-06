using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.Factory;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IEnumerable<IUserRepository> _userRepositories;
    private readonly IEnumerable<IRoleRepository> _roleRepositories;
    private readonly IEnumerable<INumberSequenceRepository> _numberSequenceRepositories;
    private readonly IEnumerable<IPermissionRepository> _permissionRepositories;
    private readonly IEnumerable<IRefreshTokenRepository> _refreshTokenRepositories;
    private readonly IEnumerable<IGoodsReceiptHeaderRepository> _goodsReceiptHeaderRepositories;
    public RepositoryFactory(IEnumerable<IUserRepository> userRepositories, IEnumerable<IRoleRepository> roleRepositories, IEnumerable<INumberSequenceRepository> numberSequenceRepositories, IEnumerable<IPermissionRepository> permissionRepositories, IEnumerable<IRefreshTokenRepository> refreshTokenRepositories, IEnumerable<IGoodsReceiptHeaderRepository> goodsReceiptHeaderRepositories)
    {
        _userRepositories = userRepositories;
        _roleRepositories = roleRepositories;
        _numberSequenceRepositories = numberSequenceRepositories;
        _permissionRepositories = permissionRepositories;
        _refreshTokenRepositories = refreshTokenRepositories;
        _goodsReceiptHeaderRepositories = goodsReceiptHeaderRepositories;
    }

    public T? GetRepository<T>(DatabaseProvider databaseProvider) where T : class
    {
        return typeof(T) switch
        {
            var type when type == typeof(IUserRepository) => _userRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IRoleRepository) => _roleRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(INumberSequenceRepository) => _numberSequenceRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IPermissionRepository) => _permissionRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IRefreshTokenRepository) => _refreshTokenRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            var type when type == typeof(IGoodsReceiptHeaderRepository) => _goodsReceiptHeaderRepositories.FirstOrDefault(x => x.DatabaseProvider == databaseProvider) as T,
            _ => throw new ArgumentOutOfRangeException(nameof(T), typeof(T), null)
        };
    }

}