using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;

public interface IDeleteWorkOrderHeaderHandler
{
    Task<ServiceResponse> Handle(int id);
}