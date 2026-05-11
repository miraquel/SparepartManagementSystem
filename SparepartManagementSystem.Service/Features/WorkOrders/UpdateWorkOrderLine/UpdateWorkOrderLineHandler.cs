using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;

public class UpdateWorkOrderLineHandler : IUpdateWorkOrderLineHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<UpdateWorkOrderLineHandler>();

    public UpdateWorkOrderLineHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderLineDto dto)
    {
        try
        {
            var record = await _unitOfWork.WorkOrderLineRepository.GetById(dto.WorkOrderLineId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Work Order Line has been modified by another user, please refresh and try again");
            }

            record.UpdateProperties(_mapper.MapToWorkOrderLine(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Work Order Line"
                };
            }

            await _unitOfWork.WorkOrderLineRepository.Update(record, _repositoryEvents.OnBeforeUpdate);

            await _unitOfWork.Commit();

            _logger.Information("Work Order Line updated successfully, Work Order Line Id: {WorkOrderLineId}", dto.WorkOrderLineId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line updated successfully"
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