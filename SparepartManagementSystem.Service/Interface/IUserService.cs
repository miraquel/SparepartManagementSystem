using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IUserService : IService<UserDto>
{
    Task<ServiceResponse<IEnumerable<UserDto>>> GetAllWithRoles();
    Task<ServiceResponse<UserDto>> GetByIdWithRoles(int id);
    Task<ServiceResponse> AddRole(UserRoleDto dto);
    Task<ServiceResponse> DeleteRole(UserRoleDto dto);
    Task<ServiceResponse<UserDto>> GetByUsernameWithRoles(string username);
    Task<ServiceResponse<TokenDto>> LoginWithActiveDirectory(UsernamePasswordDto usernamePassword);
    Task<ServiceResponse<TokenDto>> RefreshToken(string token);
    Task<ServiceResponse<RefreshTokenDto>> RevokeToken(string token);
    Task<ServiceResponse<IEnumerable<RefreshTokenDto>>> RevokeAllTokens(int userId);
    ServiceResponse<IEnumerable<ActiveDirectoryDto>> GetUsersFromActiveDirectory(string searchText);
}