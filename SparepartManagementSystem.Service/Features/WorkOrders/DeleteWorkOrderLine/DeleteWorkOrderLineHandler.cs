using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;

public class DeleteWorkOrderLineHandler : IDeleteWorkOrderLineHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<DeleteWorkOrderLineHandler>();

    public DeleteWorkOrderLineHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(int id)
    {
        try
        {
            await _unitOfWork.WorkOrderLineRepository.Delete(id);

            await _unitOfWork.Commit();

            _logger.Information("Work Order Line deleted successfully, Work Order Line Id: {WorkOrderLineId}", id);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line deleted successfully"
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