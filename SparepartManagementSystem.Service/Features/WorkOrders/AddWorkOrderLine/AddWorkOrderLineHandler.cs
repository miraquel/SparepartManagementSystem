using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;

public class AddWorkOrderLineHandler : IAddWorkOrderLineHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<AddWorkOrderLineHandler>();

    public AddWorkOrderLineHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderLineDto dto)
    {
        try
        {
            var workOrderLineAdd = _mapper.MapToWorkOrderLine(dto);
            await _unitOfWork.WorkOrderLineRepository.Add(workOrderLineAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            await _unitOfWork.Commit();

            _logger.Information("Work Order Line added successfully, Work Order Line Id: {WorkOrderLineId}", lastInsertedId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line added successfully"
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