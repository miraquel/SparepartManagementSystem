using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;

public class GetWorkOrderLineByWorkOrderHeaderIdHandler : IGetWorkOrderLineByWorkOrderHeaderIdHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetWorkOrderLineByWorkOrderHeaderIdHandler>();

    public GetWorkOrderLineByWorkOrderHeaderIdHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> Handle(int id)
    {
        try
        {
            var workOrderLines = await _unitOfWork.WorkOrderLineRepository.GetByWorkOrderHeaderId(id);

            return new ServiceResponse<IEnumerable<WorkOrderLineDto>>
            {
                Data = _mapper.MapToListOfWorkOrderLineDto(workOrderLines),
                Message = "Work Order Lines retrieved successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<WorkOrderLineDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}