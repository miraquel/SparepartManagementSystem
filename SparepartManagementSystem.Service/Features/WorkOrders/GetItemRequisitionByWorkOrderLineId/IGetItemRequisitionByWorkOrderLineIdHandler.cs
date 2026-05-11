using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId;

public interface IGetItemRequisitionByWorkOrderLineIdHandler
{
    Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> Handle(int id);
}