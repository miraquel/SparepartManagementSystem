using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;

public interface IAddWorkOrderLineHandler
{
    Task<ServiceResponse> Handle(WorkOrderLineDto dto);
}