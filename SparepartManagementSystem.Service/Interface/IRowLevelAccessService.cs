using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IRowLevelAccessService : IService<RowLevelAccessDto>
{
    Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetByUserId(int userId);
    Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> BulkDelete(IEnumerable<int> ids);
}