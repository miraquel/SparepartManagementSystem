using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IRoleRepository
{
    Task Add(Role entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(Role entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<Role>> GetAll();
    Task<Role> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<Role>> GetByParams(Dictionary<string, string> parameters);
    Task AddUser(int roleId, int userId);
    Task DeleteUser(int roleId, int userId);
    Task<IEnumerable<Role>> GetAllWithUsers();
    Task<Role> GetByIdWithUsers(int id);
    DatabaseProvider DatabaseProvider { get; }
}