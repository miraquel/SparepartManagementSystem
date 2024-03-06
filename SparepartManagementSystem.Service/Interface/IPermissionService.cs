using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface
{
    public interface IPermissionService : IService<PermissionDto>
    {
        Task<ServiceResponse<IEnumerable<PermissionDto>>> GetByRoleId(int roleId);
        ServiceResponse<IEnumerable<PermissionDto>> GetAllPermissionTypes();
        ServiceResponse<IEnumerable<PermissionDto>> GetAllModules();
    }
}
