using System.Security.Claims;
using System.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
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
        services.AddScoped<RepositoryEvents>();
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
        services.AddScoped<IVersionTrackerService, VersionTrackerService>();
        services.AddScoped<IGMKSMSServiceGroup, GMKSMSServiceGroupImplementation>();
        services.AddScoped(_ => new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(300))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600)));
        services.AddScoped(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var username = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            _ = int.TryParse(httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value, out var userId);
            var userClaimDto = new UserClaimDto
            {
                UserId = userId,
                Username = username
            };

            return userClaimDto;
        });
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