using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById;

public interface IGetItemRequisitionByIdHandler
{
    Task<ServiceResponse<ItemRequisitionDto>> Handle(int id);
}