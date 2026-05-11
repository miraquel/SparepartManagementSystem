using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;

public class AddWorkOrderHeaderWithLinesHandler : IAddWorkOrderHeaderWithLinesHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<AddWorkOrderHeaderWithLinesHandler>();

    public AddWorkOrderHeaderWithLinesHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderHeaderDto dto)
    {
        try
        {
            var workOrderHeaderAdd = _mapper.MapToWorkOrderHeader(dto);
            await _unitOfWork.WorkOrderHeaderRepository.Add(workOrderHeaderAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            var workOrderLines = _mapper.MapToListOfWorkOrderLine(dto.WorkOrderLines).ToList();

            foreach (var workOrderLine in workOrderLines)
            {
                workOrderLine.WorkOrderHeaderId = lastInsertedId;
            }

            await _unitOfWork.WorkOrderLineRepository.BulkAdd(workOrderLines, _repositoryEvents.OnBeforeAdd);

            await _unitOfWork.Commit();

            _logger.Information("Work Order Header and lines added successfully, Work Order Header Id: {WorkOrderHeaderId}", lastInsertedId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header added successfully"
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