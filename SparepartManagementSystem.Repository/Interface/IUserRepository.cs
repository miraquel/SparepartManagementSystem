using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IUserRepository
{
    Task Add(User entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(User entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<User>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<User>> GetAllWithRoles();
    Task<User> GetByIdWithRoles(int id);
    Task<User> GetByUsername(string username);
    Task<User> GetByUsernameWithRoles(string username);
    Task<User> GetByIdWithUserWarehouse(int id);
    DatabaseProvider DatabaseProvider { get; }
}