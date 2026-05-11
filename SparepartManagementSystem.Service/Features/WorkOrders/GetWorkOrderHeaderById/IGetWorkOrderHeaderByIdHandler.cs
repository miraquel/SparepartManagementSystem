using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;

public interface IGetWorkOrderHeaderByIdHandler
{
    Task<ServiceResponse<WorkOrderHeaderDto>> Handle(int id);
}