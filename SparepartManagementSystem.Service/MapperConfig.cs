using AutoMapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service;

internal static class MapperConfig
{
    public static Mapper InitializeAutoMapper()
    {
        //Provide all the Mapping Configuration
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<NumberSequence, NumberSequenceDto>();
            cfg.CreateMap<NumberSequenceDto, NumberSequence>();
            cfg.CreateMap<PermissionDto, Permission>();
            cfg.CreateMap<Permission, PermissionDto>();
            cfg.CreateMap<RefreshTokenDto, RefreshToken>();
            cfg.CreateMap<RefreshToken, RefreshTokenDto>();
            cfg.CreateMap<GoodsReceiptHeader, GoodsReceiptHeaderDto>();
            cfg.CreateMap<GoodsReceiptHeaderDto, GoodsReceiptHeader>();
            cfg.CreateMap<GoodsReceiptLine, GoodsReceiptLineDto>();
            cfg.CreateMap<GoodsReceiptLineDto, GoodsReceiptLine>();
            cfg.CreateMap<RowLevelAccess, RowLevelAccessDto>();
            cfg.CreateMap<RowLevelAccessDto, RowLevelAccess>();

            // Dynamics AX 2012 object mapping
            cfg.CreateMap<GMKInventTableDataContract, InventTableDto>();
            cfg.CreateMap<InventTableDto, GMKInventTableDataContract>();
            cfg.CreateMap<GMKPurchTableDataContract, PurchTableDto>();
            cfg.CreateMap<PurchTableDto, GMKPurchTableDataContract>();
            cfg.CreateMap<GMKPurchLineDataContract, PurchLineDto>();
            cfg.CreateMap<PurchLineDto, GMKPurchLineDataContract>();
            cfg.CreateMap<GMKWMSLocationDataContract, WMSLocationDto>();
            cfg.CreateMap<WMSLocationDto, GMKWMSLocationDataContract>();
            cfg.CreateMap<GMKServiceResponseDataContract, GMKServiceResponseDto>();
            cfg.CreateMap<GMKServiceResponseDto, GMKServiceResponseDataContract>();
            cfg.CreateMap<GMKInventSumDataContract, InventSumDto>();
            cfg.CreateMap<InventSumDto, GMKInventSumDataContract>();
            cfg.CreateMap<GMKWorkOrderDataContract, WorkOrderDto>();
            cfg.CreateMap<WorkOrderDto, GMKWorkOrderDataContract>();
        });

        //Create an Instance of Mapper and return that Instance
        var mapper = new Mapper(config);
        return mapper;
    }
}