using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById;

public class GetItemRequisitionByIdHandler : IGetItemRequisitionByIdHandler
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GetItemRequisitionByIdHandler>();

    public GetItemRequisitionByIdHandler(MapperlyMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<ItemRequisitionDto>> Handle(int id)
    {
        try
        {
            var itemRequisition = await _unitOfWork.ItemRequisitionRepository.GetById(id);

            return new ServiceResponse<ItemRequisitionDto>
            {
                Success = true,
                Data = _mapper.MapToItemRequisitionDto(itemRequisition),
                Message = "Item Requisition retrieved successfully"
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

            return new ServiceResponse<ItemRequisitionDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}