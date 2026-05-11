using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;

public class UpdateWorkOrderHeaderHandler : IUpdateWorkOrderHeaderHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<UpdateWorkOrderHeaderHandler>();

    public UpdateWorkOrderHeaderHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> Handle(WorkOrderHeaderDto dto)
    {
        try
        {
            var record = await _unitOfWork.WorkOrderHeaderRepository.GetById(dto.WorkOrderHeaderId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Work Order Header has been modified by another user, please refresh and try again");
            }

            record.UpdateProperties(_mapper.MapToWorkOrderHeader(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Work Order Header"
                };
            }

            await _unitOfWork.WorkOrderHeaderRepository.Update(record, _repositoryEvents.OnBeforeUpdate);
            await _unitOfWork.Commit();

            _logger.Information("Work Order Header updated successfully, Work Order Header Id: {WorkOrderHeaderId}", dto.WorkOrderHeaderId);

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header updated successfully"
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