﻿using System.ServiceModel;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service;

public static class ServiceCollectionExtension
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<IMapper>(_ => MapperConfig.InitializeAutoMapper());
        services.AddSingleton<PermissionTypeAccessor>();
        services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<INumberSequenceService, NumberSequenceService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IGoodsReceiptHeaderService, GoodsReceiptHeaderService>();
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
                } // Just to test, change if needed larger
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