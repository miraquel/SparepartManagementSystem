using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;

public interface IUpdateWorkOrderLineHandler
{
    Task<ServiceResponse> Handle(WorkOrderLineDto dto);
}