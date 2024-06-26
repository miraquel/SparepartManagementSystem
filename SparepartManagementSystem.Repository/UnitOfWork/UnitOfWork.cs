using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Extensions.Configuration;
using SparepartManagementSystem.Repository.Factory;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseProvider _databaseProvider;
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _dbTransaction;

    public UnitOfWork(IDbTransaction dbTransaction, IDbConnection dbConnection, IConfiguration configuration, IRepositoryFactory repositoryFactory)
    {
        _dbConnection = dbConnection;
        _dbTransaction = dbTransaction;
        
        var databaseProvider = configuration.GetSection("DatabaseProvider").Value;
        if (databaseProvider != null)
        {
            _databaseProvider = Enum.Parse<DatabaseProvider>(databaseProvider);
            UserRepository = repositoryFactory.GetRepository<IUserRepository>(_databaseProvider) ?? throw new NullReferenceException();
            UserWarehouseRepository = repositoryFactory.GetRepository<IUserWarehouseRepository>(_databaseProvider) ?? throw new NullReferenceException();
            RoleRepository = repositoryFactory.GetRepository<IRoleRepository>(_databaseProvider) ?? throw new NullReferenceException();
            PermissionRepository = repositoryFactory.GetRepository<IPermissionRepository>(_databaseProvider) ?? throw new NullReferenceException();
            NumberSequenceRepository = repositoryFactory.GetRepository<INumberSequenceRepository>(_databaseProvider) ?? throw new NullReferenceException();
            RefreshTokenRepository = repositoryFactory.GetRepository<IRefreshTokenRepository>(_databaseProvider) ?? throw new NullReferenceException();
            GoodsReceiptHeaderRepository = repositoryFactory.GetRepository<IGoodsReceiptHeaderRepository>(_databaseProvider) ?? throw new NullReferenceException();
            GoodsReceiptLineRepository = repositoryFactory.GetRepository<IGoodsReceiptLineRepository>(_databaseProvider) ?? throw new NullReferenceException();
            RowLevelAccessRepository = repositoryFactory.GetRepository<IRowLevelAccessRepository>(_databaseProvider) ?? throw new NullReferenceException();
            WorkOrderHeaderRepository = repositoryFactory.GetRepository<IWorkOrderHeaderRepository>(_databaseProvider) ?? throw new NullReferenceException();
            WorkOrderLineRepository = repositoryFactory.GetRepository<IWorkOrderLineRepository>(_databaseProvider) ?? throw new NullReferenceException();
            ItemRequisitionRepository = repositoryFactory.GetRepository<IItemRequisitionRepository>(_databaseProvider) ?? throw new NullReferenceException();
            VersionTrackerRepository = repositoryFactory.GetRepository<IVersionTrackerRepository>(_databaseProvider) ?? throw new NullReferenceException();
        }
        else
        {
            throw new ArgumentNullException(nameof(configuration), "databaseProvider is null");
        }
    }

    public IUserRepository UserRepository { get; }
    public IUserWarehouseRepository UserWarehouseRepository { get; }
    public IPermissionRepository PermissionRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public INumberSequenceRepository NumberSequenceRepository { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public IGoodsReceiptHeaderRepository GoodsReceiptHeaderRepository { get; }
    public IGoodsReceiptLineRepository GoodsReceiptLineRepository { get; }
    public IRowLevelAccessRepository RowLevelAccessRepository { get; }
    public IWorkOrderHeaderRepository WorkOrderHeaderRepository { get; }
    public IWorkOrderLineRepository WorkOrderLineRepository { get; }
    public IItemRequisitionRepository ItemRequisitionRepository { get; }
    public IVersionTrackerRepository VersionTrackerRepository { get; }

    public async Task<int> GetLastInsertedId()
    {
        switch (_databaseProvider)
        {
            case DatabaseProvider.MySql:
                const string query = "SELECT LAST_INSERT_ID()";
                var result = await _dbConnection.QueryFirstOrDefaultAsync<int>(query, transaction: _dbTransaction);
                return result;
            case DatabaseProvider.SqlServer:
            case DatabaseProvider.PostgresSql:
            case DatabaseProvider.Oracle:
            case DatabaseProvider.SqLite:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task Commit()
    {
        if (_dbTransaction is DbTransaction dbTransaction)
        {
            try
            {
                await dbTransaction.CommitAsync();
            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
            }
        }
        else
        {
            try
            {
                _dbTransaction.Commit();
                _dbConnection.BeginTransaction();
            }
            catch (Exception)
            {
                _dbTransaction.Rollback();
            }
        }
    }

    public async Task Rollback()
    {
        if (_dbTransaction is DbTransaction dbTransaction)
        {
            await dbTransaction.RollbackAsync();
        }
        else
        {
            _dbTransaction.Rollback();
        }
    }
    
    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //Close the SQL Connection and dispose the objects
                _dbConnection.Close();
                _dbConnection.Dispose();
                _dbTransaction.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_dbConnection is DbConnection dbConnection)
        {
            await dbConnection.CloseAsync();
        }
        else
        {
            _dbConnection.Close();
        }
        
        await CastAndDispose(_dbConnection);
        await CastAndDispose(_dbTransaction);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync();
            else
                resource.Dispose();
        }
    }
}