using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SparepartManagementSystem.Repository.Tests;

public class ServiceCollectionHelper
{
    private readonly ServiceProvider _serviceProvider = Services().BuildServiceProvider();

    public T GetRequiredService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
    
    public static T GetRequiredService<T>(IServiceScope scope) where T : class
    {
        return scope.ServiceProvider.GetRequiredService<T>();
    }
    
    private static ServiceCollection Services()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<IConfiguration>(_ => new ConfigurationBuilder()
            .AddUserSecrets<ServiceCollectionHelper>()
            .Build());
        services.AddSingleton<MapperlyMapper>();
        services.AddRepository();

        return services;
    }
}