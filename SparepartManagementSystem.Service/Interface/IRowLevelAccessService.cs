using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IRowLevelAccessService
{
    Task<ServiceResponse> AddRowLevelAccess(RowLevelAccessDto entity);
    Task<ServiceResponse> UpdateRowLevelAccess(RowLevelAccessDto entity);
    Task<ServiceResponse> DeleteRowLevelAccess(int id);
    Task<ServiceResponse<RowLevelAccessDto>> GetRowLevelAccessById(int id);
    Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetAllRowLevelAccess();
    Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetRowLevelAccessByParams(RowLevelAccessDto entity);
    Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetRowLevelAccessByUserId(int userId);
    Task<ServiceResponse> BulkDeleteRowLevelAccess(IEnumerable<int> ids);
}