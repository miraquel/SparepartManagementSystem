using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;

public class GetWorkOrderHeaderByIdHandler : IGetWorkOrderHeaderByIdHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetWorkOrderHeaderByIdHandler>();

    public GetWorkOrderHeaderByIdHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<WorkOrderHeaderDto>> Handle(int id)
    {
        try
        {
            var workOrderHeader = await _unitOfWork.WorkOrderHeaderRepository.GetById(id);

            return new ServiceResponse<WorkOrderHeaderDto>
            {
                Success = true,
                Data = _mapper.MapToWorkOrderHeaderDto(workOrderHeader)
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

            return new ServiceResponse<WorkOrderHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}