using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList;

public class GetWorkOrderHeaderByParamsPagedListHandler : IGetWorkOrderHeaderByParamsPagedListHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetWorkOrderHeaderByParamsPagedListHandler>();

    public GetWorkOrderHeaderByParamsPagedListHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> Handle(int pageNumber, int pageSize, Dictionary<string, string> parameters)
    {
        try
        {
            var result = await _unitOfWork.WorkOrderHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, parameters);

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Data = new PagedListDto<WorkOrderHeaderDto>(
                    _mapper.MapToListOfWorkOrderHeaderDto(result.Items),
                    result.PageNumber,
                    result.PageSize,
                    result.TotalCount),
                Message = "Work Order Headers retrieved successfully",
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

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}