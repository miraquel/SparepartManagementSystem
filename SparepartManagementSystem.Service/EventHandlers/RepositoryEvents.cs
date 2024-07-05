using SparepartManagementSystem.Repository.EventHandlers;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.EventHandlers;

public class RepositoryEvents
{
    public RepositoryEvents(UserClaimDto userClaimDto)
    {
        OnBeforeAdd = (_, e) =>
        {
            e.Entity.CreatedBy = userClaimDto.Username;
            e.Entity.CreatedDateTime = DateTime.Now;
            e.Entity.ModifiedBy = userClaimDto.Username;
            e.Entity.ModifiedDateTime = DateTime.Now;
        };
        OnBeforeUpdate = (_, e) =>
        {
            e.Entity.ModifiedBy = userClaimDto.Username;
            e.Entity.ModifiedDateTime = DateTime.Now;
        };
    }

    public EventHandler<AddEventArgs> OnBeforeAdd { get; set; }
    public EventHandler<BeforeUpdateEventArgs> OnBeforeUpdate { get; set; }
}