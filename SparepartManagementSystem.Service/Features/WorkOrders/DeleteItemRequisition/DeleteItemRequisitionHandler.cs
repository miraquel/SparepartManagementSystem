using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteItemRequisition;

public class DeleteItemRequisitionHandler : IDeleteItemRequisitionHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<DeleteItemRequisitionHandler>();

    public DeleteItemRequisitionHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(int id)
    {
        try
        {
            await _unitOfWork.ItemRequisitionRepository.Delete(id);

            _logger.Information("Item Requisition deleted successfully, Item Requisition Id: {ItemRequisitionId}", id);

            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition deleted successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}