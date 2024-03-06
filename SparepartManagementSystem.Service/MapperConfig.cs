using AutoMapper;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.DTO;

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

            // Dynamics AX 2012 object mapping

        });

        //Create an Instance of Mapper and return that Instance
        var mapper = new Mapper(config);
        return mapper;
    }
}