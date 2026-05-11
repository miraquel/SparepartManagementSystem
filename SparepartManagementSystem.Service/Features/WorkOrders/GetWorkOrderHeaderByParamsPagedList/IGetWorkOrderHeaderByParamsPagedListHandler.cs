using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList;

public interface IGetWorkOrderHeaderByParamsPagedListHandler
{
    Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> Handle(int pageNumber, int pageSize, Dictionary<string, string> parameters);
}