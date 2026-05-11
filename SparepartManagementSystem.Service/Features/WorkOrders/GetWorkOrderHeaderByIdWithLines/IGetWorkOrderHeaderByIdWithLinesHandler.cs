using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines;

public interface IGetWorkOrderHeaderByIdWithLinesHandler
{
    Task<ServiceResponse<WorkOrderHeaderDto>> Handle(int id);
}