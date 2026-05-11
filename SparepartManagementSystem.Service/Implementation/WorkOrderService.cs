using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderHeaderWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.AddWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.DeleteWorkOrderLine;
using SparepartManagementSystem.Service.Features.WorkOrders.GetAllWorkOrderHeaderPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetItemRequisitionByWorkOrderLineId;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByIdWithLines;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderHeaderByParamsPagedList;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineById;
using SparepartManagementSystem.Service.Features.WorkOrders.GetWorkOrderLineByWorkOrderHeaderId;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderHeader;
using SparepartManagementSystem.Service.Features.WorkOrders.UpdateWorkOrderLine;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class WorkOrderService : IWorkOrderService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly IAddWorkOrderHeaderHandler _addWorkOrderHeaderHandler;
    private readonly IAddWorkOrderHeaderWithLinesHandler _addWorkOrderHeaderWithLinesHandler;
    private readonly IAddWorkOrderLineHandler _addWorkOrderLineHandler;
    private readonly IDeleteWorkOrderHeaderHandler _deleteWorkOrderHeaderHandler;
    private readonly IDeleteWorkOrderLineHandler _deleteWorkOrderLineHandler;
    private readonly IGetAllWorkOrderHeaderPagedListHandler _getAllWorkOrderHeaderPagedListHandler;
    private readonly IGetItemRequisitionByIdHandler _getItemRequisitionByIdHandler;
    private readonly IGetItemRequisitionByWorkOrderLineIdHandler _getItemRequisitionByWorkOrderLineIdHandler;
    private readonly IGetWorkOrderHeaderByIdHandler _getWorkOrderHeaderByIdHandler;
    private readonly IGetWorkOrderHeaderByIdWithLinesHandler _getWorkOrderHeaderByIdWithLinesHandler;
    private readonly IGetWorkOrderHeaderByParamsPagedListHandler _getWorkOrderHeaderByParamsPagedListHandler;
    private readonly IGetWorkOrderLineByIdHandler _getWorkOrderLineByIdHandler;
    private readonly IGetWorkOrderLineByWorkOrderHeaderIdHandler _getWorkOrderLineByWorkOrderHeaderIdHandler;
    private readonly IUpdateWorkOrderHeaderHandler _updateWorkOrderHeaderHandler;
    private readonly IUpdateWorkOrderLineHandler _updateWorkOrderLineHandler;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();

    public WorkOrderService(
        MapperlyMapper mapper,
        IUnitOfWork unitOfWork,
        RepositoryEvents repositoryEvents,
        IAddWorkOrderHeaderHandler addWorkOrderHeaderHandler,
        IAddWorkOrderHeaderWithLinesHandler addWorkOrderHeaderWithLinesHandler,
        IAddWorkOrderLineHandler addWorkOrderLineHandler,
        IDeleteWorkOrderHeaderHandler deleteWorkOrderHeaderHandler,
        IDeleteWorkOrderLineHandler deleteWorkOrderLineHandler,
        IGetAllWorkOrderHeaderPagedListHandler getAllWorkOrderHeaderPagedListHandler,
        IGetItemRequisitionByIdHandler getItemRequisitionByIdHandler,
        IGetItemRequisitionByWorkOrderLineIdHandler getItemRequisitionByWorkOrderLineIdHandler,
        IGetWorkOrderHeaderByIdHandler getWorkOrderHeaderByIdHandler,
        IGetWorkOrderHeaderByIdWithLinesHandler getWorkOrderHeaderByIdWithLinesHandler,
        IGetWorkOrderHeaderByParamsPagedListHandler getWorkOrderHeaderByParamsPagedListHandler,
        IGetWorkOrderLineByIdHandler getWorkOrderLineByIdHandler,
        IGetWorkOrderLineByWorkOrderHeaderIdHandler getWorkOrderLineByWorkOrderHeaderIdHandler,
        IUpdateWorkOrderHeaderHandler updateWorkOrderHeaderHandler,
        IUpdateWorkOrderLineHandler updateWorkOrderLineHandler)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repositoryEvents = repositoryEvents;
        _addWorkOrderHeaderHandler = addWorkOrderHeaderHandler;
        _addWorkOrderHeaderWithLinesHandler = addWorkOrderHeaderWithLinesHandler;
        _addWorkOrderLineHandler = addWorkOrderLineHandler;
        _deleteWorkOrderHeaderHandler = deleteWorkOrderHeaderHandler;
        _deleteWorkOrderLineHandler = deleteWorkOrderLineHandler;
        _getAllWorkOrderHeaderPagedListHandler = getAllWorkOrderHeaderPagedListHandler;
        _getItemRequisitionByIdHandler = getItemRequisitionByIdHandler;
        _getItemRequisitionByWorkOrderLineIdHandler = getItemRequisitionByWorkOrderLineIdHandler;
        _getWorkOrderHeaderByIdHandler = getWorkOrderHeaderByIdHandler;
        _getWorkOrderHeaderByIdWithLinesHandler = getWorkOrderHeaderByIdWithLinesHandler;
        _getWorkOrderHeaderByParamsPagedListHandler = getWorkOrderHeaderByParamsPagedListHandler;
        _getWorkOrderLineByIdHandler = getWorkOrderLineByIdHandler;
        _getWorkOrderLineByWorkOrderHeaderIdHandler = getWorkOrderLineByWorkOrderHeaderIdHandler;
        _updateWorkOrderHeaderHandler = updateWorkOrderHeaderHandler;
        _updateWorkOrderLineHandler = updateWorkOrderLineHandler;
    }
    
    public Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        return _addWorkOrderHeaderHandler.Handle(dto);
    }

    public Task<ServiceResponse> AddWorkOrderHeaderWithLines(WorkOrderHeaderDto dto)
    {
        return _addWorkOrderHeaderWithLinesHandler.Handle(dto);
    }

    public Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        return _updateWorkOrderHeaderHandler.Handle(dto);
    }
    public Task<ServiceResponse> DeleteWorkOrderHeader(int id)
    {
        return _deleteWorkOrderHeaderHandler.Handle(id);
    }
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderById(int id)
    {
        return _getWorkOrderHeaderByIdHandler.Handle(id);
    }
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetAllWorkOrderHeaderPagedList(int pageNumber, int pageSize)
    {
        return _getAllWorkOrderHeaderPagedListHandler.Handle(pageNumber, pageSize);
    }
    public Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters)
    {
        return _getWorkOrderHeaderByParamsPagedListHandler.Handle(pageNumber, pageSize, parameters);
    }
    public Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        return _addWorkOrderLineHandler.Handle(dto);
    }
    public Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        return _updateWorkOrderLineHandler.Handle(dto);
    }
    public Task<ServiceResponse> DeleteWorkOrderLine(int id)
    {
        return _deleteWorkOrderLineHandler.Handle(id);
    }
    public Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLineById(int id)
    {
        return _getWorkOrderLineByIdHandler.Handle(id);
    }
    public Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineByWorkOrderHeaderId(int id)
    {
        return _getWorkOrderLineByWorkOrderHeaderIdHandler.Handle(id);
    }
    public Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderByIdWithLines(int id)
    {
        return _getWorkOrderHeaderByIdWithLinesHandler.Handle(id);
    }

    public async Task<ServiceResponse> AddItemRequisition(ItemRequisitionDto dto)
    {
        try
        {
            var itemRequisitionAdd = _mapper.MapToItemRequisition(dto);
            await _unitOfWork.ItemRequisitionRepository.Add(itemRequisitionAdd, _repositoryEvents.OnBeforeAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            _logger.Information("Item Requisition added successfully, Item Requisition Id: {ItemRequisitionId}", lastInsertedId);
            
            await _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition added successfully",
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

    public async Task<ServiceResponse> UpdateItemRequisition(ItemRequisitionDto dto)
    {
        try
        {
            var record = await _unitOfWork.ItemRequisitionRepository.GetById(dto.ItemRequisitionId, true);
            
            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Item Requisition has been modified by another user, please refresh and try again");
            }
            
            record.UpdateProperties(_mapper.MapToItemRequisition(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Item Requisition"
                };
            }
            
            await _unitOfWork.ItemRequisitionRepository.Update(record, _repositoryEvents.OnBeforeUpdate);
            
            _logger.Information("Item Requisition updated successfully, Item Requisition Id: {ItemRequisitionId}", dto.ItemRequisitionId);
            
            await _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition updated successfully",
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

    public async Task<ServiceResponse> DeleteItemRequisition(int id)
    {
        try
        {
            await _unitOfWork.ItemRequisitionRepository.Delete(id);
            
            _logger.Information("Item Requisition deleted successfully, Item Requisition Id: {ItemRequisitionId}", id);
            
            await _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition deleted successfully",
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

    public Task<ServiceResponse<ItemRequisitionDto>> GetItemRequisitionById(int id)
    {
        return _getItemRequisitionByIdHandler.Handle(id);
    }

    public async Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> GetItemRequisitionByParams(Dictionary<string, string> parameters)
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

    public Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> GetItemRequisitionByWorkOrderLineId(int id)
    {
        return _getItemRequisitionByWorkOrderLineIdHandler.Handle(id);
    }
}