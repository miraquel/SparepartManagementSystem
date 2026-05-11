using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList;

public interface IGetAllWorkOrderHeaderPagedListHandler
{
    Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> Handle(int pageNumber, int pageSize);
}