using System.Data;
using Microsoft.Extensions.Configuration;
using SparepartManagementSystem.Repository.Factory;
using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private IDbTransaction DbTransaction { get; }
    public IUserRepository UserRepository { get; }
    public IPermissionRepository PermissionRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public INumberSequenceRepository NumberSequenceRepository { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public IGoodsReceiptHeaderRepository GoodsReceiptHeaderRepository { get; }
    
    public UnitOfWork(IDbTransaction dbTransaction, IConfiguration configuration, IRepositoryFactory repositoryFactory)
    {
        DbTransaction = dbTransaction;
        
        var databaseProvider = configuration.GetSection("DatabaseProvider").Value;
        if (databaseProvider != null)
        {
            var databaseProviderEnum = Enum.Parse<DatabaseProvider>(databaseProvider);
            UserRepository = repositoryFactory.GetRepository<IUserRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
            RoleRepository = repositoryFactory.GetRepository<IRoleRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
            PermissionRepository = repositoryFactory.GetRepository<IPermissionRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
            NumberSequenceRepository = repositoryFactory.GetRepository<INumberSequenceRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
            RefreshTokenRepository = repositoryFactory.GetRepository<IRefreshTokenRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
            GoodsReceiptHeaderRepository = repositoryFactory.GetRepository<IGoodsReceiptHeaderRepository>(databaseProviderEnum) ?? throw new NullReferenceException();
        }
        else
        {
            throw new ArgumentNullException(nameof(configuration), "databaseProvider is null");
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposing) return;

        //Close the SQL Connection and dispose the objects
        DbTransaction.Connection?.Close();
        DbTransaction.Connection?.Dispose();
        DbTransaction.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Commit()
    {
        try
        {
            DbTransaction.Commit();
        }
        catch (Exception)
        {
            DbTransaction.Rollback();
        }
    }

    public void Rollback()
    {
        DbTransaction.Rollback();
    }

    
}