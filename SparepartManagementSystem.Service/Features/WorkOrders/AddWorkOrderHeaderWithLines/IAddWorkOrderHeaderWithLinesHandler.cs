using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;

public interface IAddWorkOrderHeaderWithLinesHandler
{
    Task<ServiceResponse> Handle(WorkOrderHeaderDto dto);
}