using SparepartManagementSystem.Repository.Interface;

namespace SparepartManagementSystem.Repository.UnitOfWork;

/// <summary>
/// <para>
///     <see cref="IUnitOfWork"/> interface for unit of work pattern.
/// </para>
/// <para>
///     the purpose of this interface is to process the data between the application and the database and put the serial actions in a single transaction.
/// </para>
/// <para>
///     use this interface instead of <see cref="IRepository{T}"/> derived interface to do CRUD process of the model.
/// </para>
/// <para>
///     use this interface to do CRUD process of the model.
/// </para>
/// </summary>
public interface IUnitOfWork
{
    /// <inheritdoc cref="IRoleRepository"/>
    IRoleRepository RoleRepository { get; }

    /// <inheritdoc cref="IUserRepository"/>
    IUserRepository UserRepository { get; }

    /// <inheritdoc cref="IPermissionRepository"/>
    IPermissionRepository PermissionRepository { get; }

    /// <inheritdoc cref="INumberSequenceRepository"/>
    INumberSequenceRepository NumberSequenceRepository { get; }

    /// <inheritdoc cref="IRefreshTokenRepository"/>
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    IGoodsReceiptHeaderRepository GoodsReceiptHeaderRepository { get; }
    IGoodsReceiptLineRepository GoodsReceiptLineRepository { get; }

    void Commit();
    void Rollback();
    void Dispose();
}