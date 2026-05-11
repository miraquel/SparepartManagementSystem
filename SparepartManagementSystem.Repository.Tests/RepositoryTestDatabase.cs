using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SparepartManagementSystem.Repository.Migration.Migration;
using Testcontainers.MySql;

namespace SparepartManagementSystem.Repository.Tests;

internal static class RepositoryTestDatabase
{
    private static readonly Lazy<Task<DatabaseHandle>> SharedDatabaseHandle = new(InitializeAsync);

    internal static IConfiguration CreateConfiguration()
    {
        var database = SharedDatabaseHandle.Value.GetAwaiter().GetResult();

        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = DatabaseProvider.MySql.ToString(),
                ["ConnectionStrings:MySql"] = database.ConnectionString
            })
            .Build();
    }

    private static async Task<DatabaseHandle> InitializeAsync()
    {
        var container = new MySqlBuilder("mysql:8.0")
            .WithDatabase("sparepart_management_system_tests")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        await container.StartAsync();

        var enableLocalInfile = await container.ExecAsync([
            "mysql",
            "-uroot",
            "-ptest",
            "--execute=SET GLOBAL local_infile = true;"
        ]);

        if (enableLocalInfile.ExitCode != 0)
        {
            throw new InvalidOperationException($"Failed to enable MySQL local_infile for repository tests: {enableLocalInfile.Stderr}");
        }

        var connectionStringBuilder = new MySqlConnectionStringBuilder(container.GetConnectionString())
        {
            AllowLoadLocalInfile = true,
            AllowUserVariables = true
        };

        var connectionString = connectionStringBuilder.ConnectionString;
        MigrateUp(connectionString);

        return new DatabaseHandle(container, connectionString);
    }

    private static void MigrateUp(string connectionString)
    {
        using var provider = new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
            .BuildServiceProvider();

        using var scope = provider.CreateScope();
        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();
    }

    private sealed record DatabaseHandle(MySqlContainer Container, string ConnectionString);
}