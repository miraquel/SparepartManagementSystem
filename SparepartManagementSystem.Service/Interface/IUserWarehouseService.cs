using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IUserWarehouseService
{
    Task<ServiceResponse<UserWarehouseDto>> GetUserWarehouseById(int userWarehouseId);
    Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetAllUserWarehouse();
    Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetUserWarehouseByParams(Dictionary<string, string> parameters);
    Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetUserWarehouseByUserId(int userId);
    Task<ServiceResponse> AddUserWarehouse(UserWarehouseDto userWarehouseDto);
    Task<ServiceResponse> UpdateUserWarehouse(UserWarehouseDto userWarehouseDto);
    Task<ServiceResponse> DeleteUserWarehouse(int userWarehouseId);
    Task<ServiceResponse<UserWarehouseDto>> GetDefaultUserWarehouseByUserId(int userId);
}