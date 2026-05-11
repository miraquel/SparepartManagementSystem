using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;

public interface IGetWorkOrderLineByWorkOrderHeaderIdHandler
{
    Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> Handle(int id);
}