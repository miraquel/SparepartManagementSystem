using System.Data;
using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Interface;

public interface IRowLevelAccessRepository : IRepository<RowLevelAccess>
{
    // get by UserId
    Task<IEnumerable<RowLevelAccess>> GetByUserId(int userId);
    
    
}