using Riok.Mapperly.Abstractions;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.Mapper;

[Mapper]
public partial class MapperlyMapper
{
    // Goods Receipt Header
    public partial GoodsReceiptHeaderDto MapToGoodsReceiptHeaderDto(GoodsReceiptHeader goodsReceiptHeader);
    public partial GoodsReceiptHeader MapToGoodsReceiptHeader(GoodsReceiptHeaderDto goodsReceiptHeaderDto);
    public partial IEnumerable<GoodsReceiptHeaderDto> MapToListOfGoodsReceiptHeaderDto(IEnumerable<GoodsReceiptHeader> source);

    public partial IEnumerable<GoodsReceiptHeader> MapToListOfGoodsReceiptHeader(IEnumerable<GoodsReceiptHeaderDto> source);
    
    // Goods Receipt Line
    public partial GoodsReceiptLineDto MapToGoodsReceiptLineDto(GoodsReceiptLine goodsReceiptLine);
    public partial GoodsReceiptLine MapToGoodsReceiptLine(GoodsReceiptLineDto goodsReceiptLineDto);
    public partial IEnumerable<GoodsReceiptLineDto> MapToListOfGoodsReceiptLineDto(IEnumerable<GoodsReceiptLine> source);
    public partial IEnumerable<GoodsReceiptLine> MapToListOfGoodsReceiptLine(IEnumerable<GoodsReceiptLineDto> source);
    
    // User
    public partial UserDto MapToUserDto(User user);
    public partial User MapToUser(UserDto userDto);
    public partial IEnumerable<UserDto> MapToListOfUserDto(IEnumerable<User> source);
    public partial IEnumerable<User> MapToListOfUser(IEnumerable<UserDto> source);
    
    // User Warehouse
    public partial UserWarehouseDto MapToUserWarehouseDto(UserWarehouse userWarehouse);
    public partial UserWarehouse MapToUserWarehouse(UserWarehouseDto userWarehouseDto);
    public partial IEnumerable<UserWarehouseDto> MapToListOfUserWarehouseDto(IEnumerable<UserWarehouse> source);
    public partial IEnumerable<UserWarehouse> MapToListOfUserWarehouse(IEnumerable<UserWarehouseDto> source);
    
    // Role
    public partial RoleDto MapToRoleDto(Role role);
    public partial Role MapToRole(RoleDto roleDto);
    public partial IEnumerable<RoleDto> MapToListOfRoleDto(IEnumerable<Role> source);
    public partial IEnumerable<Role> MapToListOfRole(IEnumerable<RoleDto> source);
    
    // Number Sequence
    public partial NumberSequenceDto MapToNumberSequenceDto(NumberSequence numberSequence);
    public partial NumberSequence MapToNumberSequence(NumberSequenceDto numberSequenceDto);
    public partial IEnumerable<NumberSequenceDto> MapToListOfNumberSequenceDto(IEnumerable<NumberSequence> source);
    public partial IEnumerable<NumberSequence> MapToListOfNumberSequence(IEnumerable<NumberSequenceDto> source);
    
    // Work Order Header
    public partial WorkOrderHeaderDto MapToWorkOrderHeaderDto(WorkOrderHeader workOrderHeader);
    public partial WorkOrderHeader MapToWorkOrderHeader(WorkOrderHeaderDto workOrderHeaderDto);
    public partial IEnumerable<WorkOrderHeaderDto> MapToListOfWorkOrderHeaderDto(IEnumerable<WorkOrderHeader> source);
    public partial IEnumerable<WorkOrderHeader> MapToListOfWorkOrderHeader(IEnumerable<WorkOrderHeaderDto> source);
    
    // Work Order Line
    public partial WorkOrderLineDto MapToWorkOrderLineDto(WorkOrderLine workOrderLine);
    public partial WorkOrderLine MapToWorkOrderLine(WorkOrderLineDto workOrderLineDto);
    public partial IEnumerable<WorkOrderLineDto> MapToListOfWorkOrderLineDto(IEnumerable<WorkOrderLine> source);
    public partial IEnumerable<WorkOrderLine> MapToListOfWorkOrderLine(IEnumerable<WorkOrderLineDto> source);
    
    // Item Requisition
    public partial ItemRequisitionDto MapToItemRequisitionDto(ItemRequisition itemRequisition);
    public partial ItemRequisition MapToItemRequisition(ItemRequisitionDto itemRequisitionDto);
    public partial IEnumerable<ItemRequisitionDto> MapToListOfItemRequisitionDto(IEnumerable<ItemRequisition> source);
    public partial IEnumerable<ItemRequisition> MapToListOfItemRequisition(IEnumerable<ItemRequisitionDto> source);
    
    // Refresh Token
    public partial RefreshTokenDto MapToRefreshTokenDto(RefreshToken refreshToken);
    public partial RefreshToken MapToRefreshToken(RefreshTokenDto refreshTokenDto);
    public partial IEnumerable<RefreshTokenDto> MapToListOfRefreshTokenDto(IEnumerable<RefreshToken> source);
    public partial IEnumerable<RefreshToken> MapToListOfRefreshToken(IEnumerable<RefreshTokenDto> source);
    
    // Row Level Access
    public partial RowLevelAccessDto MapToRowLevelAccessDto(RowLevelAccess rowLevelAccess);
    public partial RowLevelAccess MapToRowLevelAccess(RowLevelAccessDto rowLevelAccessDto);
    public partial IEnumerable<RowLevelAccessDto> MapToListOfRowLevelAccessDto(IEnumerable<RowLevelAccess> source);
    public partial IEnumerable<RowLevelAccess> MapToListOfRowLevelAccess(IEnumerable<RowLevelAccessDto> source);
    
    // Permission
    public partial PermissionDto MapToPermissionDto(Permission permission);
    public partial Permission MapToPermission(PermissionDto permissionDto);
    public partial IEnumerable<PermissionDto> MapToListOfPermissionDto(IEnumerable<Permission> source);
    public partial IEnumerable<Permission> MapToListOfPermission(IEnumerable<PermissionDto> source);
    
    // Dynamics AX 2012 object mapping
    
    // Invent Table
    public partial InventTableDto MapToInventTableDto(GMKInventTableDataContract gmkInventTableDataContract);
    public partial GMKInventTableDataContract MapToGMKInventTableDataContract(InventTableDto inventTableDto);
    public partial IEnumerable<InventTableDto> MapToListOfInventTableDto(IEnumerable<GMKInventTableDataContract> source);
    public partial IEnumerable<GMKInventTableDataContract> MapToListOfGMKInventTableDataContract(IEnumerable<InventTableDto> source);
    
    // Purch Table
    public partial PurchTableDto MapToPurchTableDto(GMKPurchTableDataContract gmkPurchTableDataContract);
    public partial GMKPurchTableDataContract MapToGMKPurchTableDataContract(PurchTableDto purchTableDto);
    public partial IEnumerable<PurchTableDto> MapToListOfPurchTableDto(IEnumerable<GMKPurchTableDataContract> source);
    public partial IEnumerable<GMKPurchTableDataContract> MapToListOfGMKPurchTableDataContract(IEnumerable<PurchTableDto> source);
    
    // Purch Line
    public partial PurchLineDto MapToPurchLineDto(GMKPurchLineDataContract gmkPurchLineDataContract);
    public partial GMKPurchLineDataContract MapToGMKPurchLineDataContract(PurchLineDto purchLineDto);
    public partial IEnumerable<PurchLineDto> MapToListOfPurchLineDto(IEnumerable<GMKPurchLineDataContract> source);
    public partial IEnumerable<GMKPurchLineDataContract> MapToListOfGMKPurchLineDataContract(IEnumerable<PurchLineDto> source);
    
    // WMS Location
    public partial WMSLocationDto MapToWMSLocationDto(GMKWMSLocationDataContract gmkWMSLocationDataContract);
    public partial GMKWMSLocationDataContract MapToGMKWMSLocationDataContract(WMSLocationDto wmsLocationDto);
    public partial IEnumerable<WMSLocationDto> MapToListOfWMSLocationDto(IEnumerable<GMKWMSLocationDataContract> source);
    public partial IEnumerable<GMKWMSLocationDataContract> MapToListOfGMKWMSLocationDataContract(IEnumerable<WMSLocationDto> source);
    
    // GMK Service Response
    public partial GMKServiceResponseDto MapToGMKServiceResponseDto(GMKServiceResponseDataContract gmkServiceResponseDataContract);
    public partial GMKServiceResponseDataContract MapToGMKServiceResponseDataContract(GMKServiceResponseDto gmkServiceResponseDto);
    public partial IEnumerable<GMKServiceResponseDto> MapToListOfGMKServiceResponseDto(IEnumerable<GMKServiceResponseDataContract> source);
    public partial IEnumerable<GMKServiceResponseDataContract> MapToListOfGMKServiceResponseDataContract(IEnumerable<GMKServiceResponseDto> source);
    
    // Invent Sum
    public partial InventSumDto MapToInventSumDto(GMKInventSumDataContract gmkInventSumDataContract);
    public partial GMKInventSumDataContract MapToGMKInventSumDataContract(InventSumDto inventSumDto);
    public partial IEnumerable<InventSumDto> MapToListOfInventSumDto(IEnumerable<GMKInventSumDataContract> source);
    public partial IEnumerable<GMKInventSumDataContract> MapToListOfGMKInventSumDataContract(IEnumerable<InventSumDto> source);
    
    // Work Order Header AX
    [Obsolete("Use WorkOrderHeaderDto instead", true)]
    public partial WorkOrderAxDto MapToWorkOrderAxDto(GMKWorkOrderDataContract gmkWorkOrderDataContract);
    [Obsolete("Use WorkOrderHeaderDto instead", true)]
    public partial GMKWorkOrderDataContract MapToGMKWorkOrderDataContract(WorkOrderAxDto workOrderAxDto);
    [Obsolete("Use WorkOrderHeaderDto instead", true)]
    public partial IEnumerable<WorkOrderAxDto> MapToListOfWorkOrderAxDto(IEnumerable<GMKWorkOrderDataContract> source);
    [Obsolete("Use WorkOrderHeaderDto instead", true)]
    public partial IEnumerable<GMKWorkOrderDataContract> MapToListOfGMKWorkOrderDataContract(IEnumerable<WorkOrderAxDto> source);
    
    // Work Order Line AX
    [Obsolete("Use WorkOrderLineDto instead", true)]
    public partial WorkOrderLineAxDto MapToWorkOrderLineAxDto(GMKWorkOrderLineDataContract gmkWorkOrderLineDataContract);
    [Obsolete("Use WorkOrderLineDto instead", true)]
    public partial GMKWorkOrderLineDataContract MapToGMKWorkOrderLineDataContract(WorkOrderLineAxDto workOrderLineAxDto);
    [Obsolete("Use WorkOrderLineDto instead", true)]
    public partial IEnumerable<WorkOrderLineAxDto> MapToListOfWorkOrderLineAxDto(IEnumerable<GMKWorkOrderLineDataContract> source);
    [Obsolete("Use WorkOrderLineDto instead", true)]
    public partial IEnumerable<GMKWorkOrderLineDataContract> MapToListOfGMKWorkOrderLineDataContract(IEnumerable<WorkOrderLineAxDto> source);
    
    // Work Order Header Direct
    public partial WorkOrderHeaderDto MapToWorkOrderHeaderDto(GMKWorkOrderDataContract gmkWorkOrderDataContract);
    public partial GMKWorkOrderDataContract MapToGMKWorkOrderDataContract(WorkOrderHeaderDto workOrderHeaderDto);
    public partial IEnumerable<WorkOrderHeaderDto> MapToListOfWorkOrderHeaderDto(IEnumerable<GMKWorkOrderDataContract> source);
    public partial IEnumerable<GMKWorkOrderDataContract> MapToListOfGMKWorkOrderDataContract(IEnumerable<WorkOrderHeaderDto> source);
    
    // Work Order Line Direct
    public partial WorkOrderLineDto MapToWorkOrderLineDto(GMKWorkOrderLineDataContract gmkWorkOrderLineDataContract);
    public partial GMKWorkOrderLineDataContract MapToGMKWorkOrderLineDataContract(WorkOrderLineDto workOrderLineDto);
    public partial IEnumerable<WorkOrderLineDto> MapToListOfWorkOrderLineDto(IEnumerable<GMKWorkOrderLineDataContract> source);
    public partial IEnumerable<GMKWorkOrderLineDataContract> MapToListOfGMKWorkOrderLineDataContract(IEnumerable<WorkOrderLineDto> source);
    
    // Invent Location
    public partial InventLocationDto MapToInventLocationDto(GMKInventLocationDataContract gmkInventLocationDataContract);
    public partial GMKInventLocationDataContract MapToGMKInventLocationDataContract(InventLocationDto inventLocationDto);
    public partial IEnumerable<InventLocationDto> MapToListOfInventLocationDto(IEnumerable<GMKInventLocationDataContract> source);
    public partial IEnumerable<GMKInventLocationDataContract> MapToListOfGMKInventLocationDataContract(IEnumerable<InventLocationDto> source);
    
    // Invent Req
    public partial InventReqDto MapToInventReqDto(GMKInventReqDataContract gmkInventReqDataContract);
    
    public partial GMKInventReqDataContract MapToGMKInventReqDataContract(InventReqDto inventReqDto);
    public partial IEnumerable<InventReqDto> MapToListOfInventReqDto(IEnumerable<GMKInventReqDataContract> source);
    public partial IEnumerable<GMKInventReqDataContract> MapToListOfGMKInventReqDataContract(IEnumerable<InventReqDto> source);
}