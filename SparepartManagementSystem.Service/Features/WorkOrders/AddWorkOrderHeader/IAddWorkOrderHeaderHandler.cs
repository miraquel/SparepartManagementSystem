using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;

public interface IAddWorkOrderHeaderHandler
{
    Task<ServiceResponse> Handle(WorkOrderHeaderDto dto);
}