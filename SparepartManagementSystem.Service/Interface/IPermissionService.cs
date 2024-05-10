using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface
{
    public interface IPermissionService
    {
        Task<ServiceResponse> AddPermission(PermissionDto entity);
        Task<ServiceResponse> UpdatePermission(PermissionDto entity);
        Task<ServiceResponse> DeletePermission(int id);
        Task<ServiceResponse<PermissionDto>> GetPermissionById(int id);
        Task<ServiceResponse<IEnumerable<PermissionDto>>> GetAllPermission();
        Task<ServiceResponse<IEnumerable<PermissionDto>>> GetPermissionByParams(PermissionDto entity);
        Task<ServiceResponse<IEnumerable<PermissionDto>>> GetByRoleId(int roleId);
        ServiceResponse<IEnumerable<PermissionDto>> GetAllPermissionTypes();
        ServiceResponse<IEnumerable<PermissionDto>> GetAllModules();
    }
}
