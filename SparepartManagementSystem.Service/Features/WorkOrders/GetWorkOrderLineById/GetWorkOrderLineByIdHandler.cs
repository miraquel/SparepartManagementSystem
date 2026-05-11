using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;

public class GetWorkOrderLineByIdHandler : IGetWorkOrderLineByIdHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetWorkOrderLineByIdHandler>();

    public GetWorkOrderLineByIdHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<WorkOrderLineDto>> Handle(int id)
    {
        try
        {
            var workOrderLine = await _unitOfWork.WorkOrderLineRepository.GetById(id);

            return new ServiceResponse<WorkOrderLineDto>
            {
                Success = true,
                Data = _mapper.MapToWorkOrderLineDto(workOrderLine)
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

            return new ServiceResponse<WorkOrderLineDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}