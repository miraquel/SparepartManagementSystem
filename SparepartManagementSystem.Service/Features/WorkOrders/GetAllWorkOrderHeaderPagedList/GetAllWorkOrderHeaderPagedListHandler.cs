using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList;

public class GetAllWorkOrderHeaderPagedListHandler : IGetAllWorkOrderHeaderPagedListHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetAllWorkOrderHeaderPagedListHandler>();

    public GetAllWorkOrderHeaderPagedListHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> Handle(int pageNumber, int pageSize)
    {
        try
        {
            var result = await _unitOfWork.WorkOrderHeaderRepository.GetAllPagedList(pageNumber, pageSize);

            _logger.Information("Work Order Headers fetched successfully, Total Count: {TotalCount}", result.TotalCount);

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