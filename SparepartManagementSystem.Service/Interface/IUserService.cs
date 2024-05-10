using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IUserService
{
    Task<ServiceResponse> AddUser(UserDto entity);
    Task<ServiceResponse> UpdateUser(UserDto entity);
    Task<ServiceResponse> DeleteUser(int id);
    Task<ServiceResponse<UserDto>> GetUserById(int id);
    Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUser();
    Task<ServiceResponse<IEnumerable<UserDto>>> GetUserByParams(UserDto entity);
    Task<ServiceResponse<IEnumerable<UserDto>>> GetAllWithRoles();
    Task<ServiceResponse<UserDto>> GetUserByIdWithRoles(int id);
    Task<ServiceResponse> AddRoleToUser(UserRoleDto dto);
    Task<ServiceResponse> DeleteRoleFromUser(UserRoleDto dto);
    Task<ServiceResponse<UserDto>> GetUserByUsernameWithRoles(string username);
    Task<ServiceResponse<TokenDto>> LoginWithActiveDirectory(UsernamePasswordDto usernamePassword);
    Task<ServiceResponse<TokenDto>> RefreshToken(string token);
    Task<ServiceResponse> RevokeToken(string token);
    Task<ServiceResponse> RevokeAllTokens(int userId);
    Task<ServiceResponse<UserDto>> GetUserByIdWithUserWarehouse(int id);
    ServiceResponse<IEnumerable<ActiveDirectoryDto>> GetUsersFromActiveDirectory(string searchText);
}