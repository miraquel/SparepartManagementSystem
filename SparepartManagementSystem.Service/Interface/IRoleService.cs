using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IRoleService
{
    Task<ServiceResponse> AddRole(RoleDto entity);
    Task<ServiceResponse> UpdateRole(RoleDto entity);
    Task<ServiceResponse> DeleteRole(int id);
    Task<ServiceResponse<RoleDto>> GetRoleById(int id);
    Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllRole();
    Task<ServiceResponse<IEnumerable<RoleDto>>> GetRoleByParams(Dictionary<string, string> parameters);
    Task<ServiceResponse> AddUserToRole(UserRoleDto dto);
    Task<ServiceResponse> DeleteUserFromRole(UserRoleDto dto);
    Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllRoleWithUsers();
    Task<ServiceResponse<RoleDto>> GetRoleByIdWithUsers(int id);
}