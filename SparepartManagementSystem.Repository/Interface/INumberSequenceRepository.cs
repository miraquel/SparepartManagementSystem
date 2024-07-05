using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Interface
{
    public interface INumberSequenceRepository
    {
        Task Add(NumberSequence entity, EventHandler<AddEventArgs>? onBeforeAdd = null, EventHandler<AddEventArgs>? onAfterAdd = null);
        Task Delete(int id);
        Task Update(NumberSequence entity, EventHandler<BeforeUpdateEventArgs>? onBeforeUpdate = null,
            EventHandler<AfterUpdateEventArgs>? onAfterUpdate = null);
        Task<IEnumerable<NumberSequence>> GetAll();
        Task<NumberSequence> GetById(int id, bool forUpdate = false);
        Task<IEnumerable<NumberSequence>> GetByParams(Dictionary<string, string> parameters);
        Task<string> GetNextNumberByModule(string module);
        DatabaseProvider DatabaseProvider { get; }
    }
}
