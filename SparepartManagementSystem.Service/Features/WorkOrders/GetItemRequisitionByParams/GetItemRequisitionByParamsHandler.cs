using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByParams;

public class GetItemRequisitionByParamsHandler : IGetItemRequisitionByParamsHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetItemRequisitionByParamsHandler>();

    public GetItemRequisitionByParamsHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> Handle(Dictionary<string, string> parameters)
    {
        try
        {
            var itemRequisitions = await _unitOfWork.ItemRequisitionRepository.GetByParams(parameters);
            return new ServiceResponse<IEnumerable<ItemRequisitionDto>>
            {
                Success = true,
                Data = _mapper.MapToListOfItemRequisitionDto(itemRequisitions),
                Message = "Item Requisitions retrieved successfully"
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

            return new ServiceResponse<IEnumerable<ItemRequisitionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}