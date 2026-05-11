using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteItemRequisition;

public interface IDeleteItemRequisitionHandler
{
    Task<ServiceResponse> Handle(int id);
}