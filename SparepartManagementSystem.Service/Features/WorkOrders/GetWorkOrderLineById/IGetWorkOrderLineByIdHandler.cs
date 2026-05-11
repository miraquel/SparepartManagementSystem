using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;

public interface IGetWorkOrderLineByIdHandler
{
    Task<ServiceResponse<WorkOrderLineDto>> Handle(int id);
}