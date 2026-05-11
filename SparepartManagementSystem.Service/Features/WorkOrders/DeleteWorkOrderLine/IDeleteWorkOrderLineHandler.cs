using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;

public interface IDeleteWorkOrderLineHandler
{
    Task<ServiceResponse> Handle(int id);
}