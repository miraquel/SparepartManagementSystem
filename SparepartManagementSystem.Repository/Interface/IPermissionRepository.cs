using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IPermissionRepository
{
    Task Add(Permission entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(Permission entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
        EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<Permission>> GetAll();
    Task<Permission> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<Permission>> GetByParams(Dictionary<string, string> parameters);
    Task<IEnumerable<Permission>> GetByRoleId(int roleId);
    DatabaseProvider DatabaseProvider { get; }
}