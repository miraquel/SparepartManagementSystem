using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId;

public class GetItemRequisitionByWorkOrderLineIdHandler : IGetItemRequisitionByWorkOrderLineIdHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetItemRequisitionByWorkOrderLineIdHandler>();

    public GetItemRequisitionByWorkOrderLineIdHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> Handle(int id)
    {
        try
        {
            var itemRequisitions = await _unitOfWork.ItemRequisitionRepository.GetByWorkOrderLineId(id);

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