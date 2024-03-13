using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SparepartManagementSystem.Repository.Factory;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.MySql;
using SparepartManagementSystem.Repository.UnitOfWork;
using NumberSequenceRepositoryMySql = SparepartManagementSystem.Repository.MySql.NumberSequenceRepositoryMySql;

namespace SparepartManagementSystem.Repository;

public static class ServiceCollectionExtensions
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepositoryMySql>();
        services.AddScoped<IRoleRepository, RoleRepositoryMySql>();
        services.AddScoped<IPermissionRepository, PermissionRepositoryMySql>();
        services.AddScoped<INumberSequenceRepository, NumberSequenceRepositoryMySql>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepositoryMySql>();
        services.AddScoped<IGoodsReceiptHeaderRepository, GoodsReceiptHeaderRepositoryMySql>();
        services.AddScoped<IGoodsReceiptLineRepository, GoodsReceiptLineRepositoryMySql>();

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