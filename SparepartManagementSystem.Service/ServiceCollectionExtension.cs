using System.ServiceModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service;

public static class ServiceCollectionExtension
{
    public static void AddService(this IServiceCollection services)
    {
        //services.AddScoped<IMapper>(_ => MapperConfig.InitializeAutoMapper());
        // Mapperly
        services.AddScoped(_ => new MapperlyMapper());
        
        services.AddSingleton<PermissionTypeAccessor>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserWarehouseService, UserWarehouseService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<INumberSequenceService, NumberSequenceService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IGoodsReceiptService, GoodsReceiptService>();
        services.AddScoped<IRowLevelAccessService, RowLevelAccessService>();
        services.AddScoped<IWorkOrderService, WorkOrderService>();
        services.AddScoped<IWorkOrderServiceDirect, WorkOrderServiceDirect>();
        services.AddScoped<IGMKSMSServiceGroup, GMKSMSServiceGroupImplementation>();
        services.AddScoped<GMKSMSService, GMKSMSServiceClient>(serviceProvider =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var urlConfig = config["DynamicsAXIntegration:Url"] ?? throw new InvalidOperationException("DynamicsAXIntegration:Url is not exists in configuration");
            var endpointIdentityConfig = config["DynamicsAXIntegration:EndpointIdentity"] ?? throw new InvalidOperationException("DynamicsAXIntegration:EndpointIdentity is not exists in configuration");
            var binding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 2000000,
                Security =
                {
                    Mode = SecurityMode.Transport,
                    Transport =
                    {
                        ClientCredentialType = TcpClientCredentialType.Windows
                    }
                }
            };

            var endpointIdentity = new UpnEndpointIdentity(endpointIdentityConfig);

            var uri = new Uri(urlConfig);
            var endpointAddress = new EndpointAddress(uri, endpointIdentity); // You can see "UpnEndpointIdentity" referenced here.
            return new GMKSMSServiceClient(binding, endpointAddress);
        });
        services.AddScoped(_ => new CallContext
        {
            Company = "GMK"
        });
    }
}