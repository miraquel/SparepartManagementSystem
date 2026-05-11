using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByParams;

public interface IGetItemRequisitionByParamsHandler
{
    Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> Handle(Dictionary<string, string> parameters);
}