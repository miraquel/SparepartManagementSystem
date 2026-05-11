using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;

public class DeleteWorkOrderHeaderHandler : IDeleteWorkOrderHeaderHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<DeleteWorkOrderHeaderHandler>();

    public DeleteWorkOrderHeaderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> Handle(int id)
    {
        try
        {
            await _unitOfWork.WorkOrderHeaderRepository.Delete(id);

            await _unitOfWork.Commit();

            _logger.Information("Work Order Header deleted successfully, Work Order Header Id: {WorkOrderHeaderId}", id);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header deleted successfully"
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