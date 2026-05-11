using System.Security.Claims;
using System.ServiceModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service;

public static class ServiceCollectionExtension
{
    public static void AddService(this IServiceCollection services)
    {
        // Mapperly
        services.AddScoped(_ => new MapperlyMapper());
        
        services.AddScoped<RepositoryEvents>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserWarehouseService, UserWarehouseService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<INumberSequenceService, NumberSequenceService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IGoodsReceiptService, GoodsReceiptService>();
        services.AddScoped<IRowLevelAccessService, RowLevelAccessService>();
        services.AddScoped<IAddWorkOrderHeaderHandler, AddWorkOrderHeaderHandler>();
        services.AddScoped<IAddWorkOrderHeaderWithLinesHandler, AddWorkOrderHeaderWithLinesHandler>();
        services.AddScoped<IAddWorkOrderLineHandler, AddWorkOrderLineHandler>();
        services.AddScoped<IDeleteWorkOrderHeaderHandler, DeleteWorkOrderHeaderHandler>();
        services.AddScoped<IDeleteWorkOrderLineHandler, DeleteWorkOrderLineHandler>();
        services.AddScoped<IGetAllWorkOrderHeaderPagedListHandler, GetAllWorkOrderHeaderPagedListHandler>();
        services.AddScoped<IGetItemRequisitionByIdHandler, GetItemRequisitionByIdHandler>();
        services.AddScoped<IGetItemRequisitionByWorkOrderLineIdHandler, GetItemRequisitionByWorkOrderLineIdHandler>();
        services.AddScoped<IGetWorkOrderHeaderByIdHandler, GetWorkOrderHeaderByIdHandler>();
        services.AddScoped<IGetWorkOrderHeaderByIdWithLinesHandler, GetWorkOrderHeaderByIdWithLinesHandler>();
        services.AddScoped<IGetWorkOrderHeaderByParamsPagedListHandler, GetWorkOrderHeaderByParamsPagedListHandler>();
        services.AddScoped<IGetWorkOrderLineByIdHandler, GetWorkOrderLineByIdHandler>();
        services.AddScoped<IGetWorkOrderLineByWorkOrderHeaderIdHandler, GetWorkOrderLineByWorkOrderHeaderIdHandler>();
        services.AddScoped<IUpdateWorkOrderHeaderHandler, UpdateWorkOrderHeaderHandler>();
        services.AddScoped<IUpdateWorkOrderLineHandler, UpdateWorkOrderLineHandler>();
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
                ReceiveTimeout = TimeSpan.FromMinutes(5),
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