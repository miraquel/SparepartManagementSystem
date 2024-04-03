using System.Security.Claims;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
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

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString("Data Source=:memory:")
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        
        services.AddSingleton<IConfiguration>(_ =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Test.json", true, true)
                .Build();
            return configuration;
        });
        services.AddService();
        services.AddRepository();
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