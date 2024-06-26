using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface;

public interface IVersionTrackerRepository
{
    Task Add(VersionTracker entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
    Task Delete(int id);
    Task Update(VersionTracker entity, EventHandler<UpdateEventArgs>? onBeforeUpdate = null, EventHandler<UpdateEventArgs>? onAfterUpdate = null);
    Task<IEnumerable<VersionTracker>> GetAll();
    Task<VersionTracker> GetById(int id, bool forUpdate = false);
    Task<IEnumerable<VersionTracker>> GetByParams(Dictionary<string, string> parameters);
    Task<VersionTracker> GetLatestVersionTracker();
    Task<VersionTracker> GetByVersion(string version);
    DatabaseProvider DatabaseProvider { get; }
}