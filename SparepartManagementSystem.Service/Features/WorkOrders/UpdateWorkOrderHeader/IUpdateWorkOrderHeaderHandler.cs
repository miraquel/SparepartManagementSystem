using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;

public interface IUpdateWorkOrderHeaderHandler
{
    Task<ServiceResponse> Handle(WorkOrderHeaderDto dto);
}