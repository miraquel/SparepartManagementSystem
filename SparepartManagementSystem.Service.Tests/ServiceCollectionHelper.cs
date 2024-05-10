using System.Data;
using System.Data.SQLite;
using System.Security.Claims;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SparepartManagementSystem.Repository;
using SparepartManagementSystem.Repository.Migration.Migration;

namespace SparepartManagementSystem.Service.Tests;

internal class ServiceCollectionHelper
{
    private ServiceProvider ServiceProvider { get; } = Services().BuildServiceProvider();

    public T GetRequiredService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    
    private static ServiceCollection Services()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<IConfiguration>(_ =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Test.json", true, true)
                .Build();
            return configuration;
        });
        services.AddRepository();
        services.AddService();
        services.AddScoped<IDbConnection>(_ => new SQLiteConnection("Data Source=:memory:"));
        services.AddScoped<IDbTransaction>(sp =>
        {
            var connection = sp.GetRequiredService<IDbConnection>();
            connection.Open();
            using var command = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection as SQLiteConnection);
            command.ExecuteNonQuery();
            return connection.BeginTransaction();
        });
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                //.WithGlobalConnectionString(_ => "Data Source=:memory:")
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        services.AddScoped(_ =>
        {
            var mock = new Mock<IHttpContextAccessor>();
            mock.SetupGet(contextAccessor => contextAccessor.HttpContext).Returns(new DefaultHttpContext());
            // setup user claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "test"),
                new(ClaimTypes.Name, "test"),
                new(ClaimTypes.Email, "")
            }.AsEnumerable();
            mock.SetupGet(contextAccessor => contextAccessor.HttpContext!.User.Claims).Returns(claims);
            return mock.Object;
        });

        return services;
    }
}