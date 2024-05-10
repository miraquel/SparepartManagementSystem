using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SparepartManagementSystem.Repository.Factory;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.MySql;
using SparepartManagementSystem.Repository.UnitOfWork;
using PermissionRepositoryMySql = SparepartManagementSystem.Repository.MySql.PermissionRepositoryMySql;
using RefreshTokenRepositoryMySql = SparepartManagementSystem.Repository.MySql.RefreshTokenRepositoryMySql;
using RoleRepositoryMySql = SparepartManagementSystem.Repository.MySql.RoleRepositoryMySql;
using RowLevelAccessRepositoryMysql = SparepartManagementSystem.Repository.MySql.RowLevelAccessRepositoryMysql;
using UserRepositoryMySql = SparepartManagementSystem.Repository.MySql.UserRepositoryMySql;

namespace SparepartManagementSystem.Repository;

public static class ServiceCollectionExtensions
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepositoryMySql>();
        services.AddScoped<IUserWarehouseRepository, UserWarehouseRepositoryMySql>();
        services.AddScoped<IRoleRepository, RoleRepositoryMySql>();
        services.AddScoped<IPermissionRepository, PermissionRepositoryMySql>();
        services.AddScoped<INumberSequenceRepository, NumberSequenceRepositoryMySql>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositoryMySql>();
        services.AddScoped<IGoodsReceiptHeaderRepository, GoodsReceiptHeaderRepositoryMySql>();
        services.AddScoped<IGoodsReceiptLineRepository, GoodsReceiptLineRepositoryMySql>();
        services.AddScoped<IRowLevelAccessRepository, RowLevelAccessRepositoryMysql>();
        services.AddScoped<IWorkOrderHeaderRepository, WorkOrderHeaderRepositoryMySql>();
        services.AddScoped<IWorkOrderLineRepository, WorkOrderLineRepositoryMySql>();
        services.AddScoped<IItemRequisitionRepository, ItemRequisitionRepositoryMySql>();

        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        
        services.AddScoped<IDbConnection>(s =>
        {
            var config = s.GetRequiredService<IConfiguration>();

            var database = config.GetSection("DatabaseProvider").Value ?? throw new InvalidOperationException("Database configuration is empty");
            var connectionString = config.GetConnectionString(database);
            return database switch
            {
                //"SqlServer" => new SqlConnection(connectionString),
                "MySql" => new MySqlConnection(connectionString),
                "SqLite" => new SQLiteConnection(connectionString),
                _ => new MySqlConnection(connectionString)
            };
        });
        services.AddScoped(s =>
        {
            var conn = s.GetRequiredService<IDbConnection>();
            conn.Open();
            return conn.BeginTransaction();
        });
    }
}