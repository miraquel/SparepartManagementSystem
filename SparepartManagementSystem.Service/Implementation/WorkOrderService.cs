using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class WorkOrderService : IWorkOrderService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkOrderService(MapperlyMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        try
        {
            var workOrderHeaderAdd = _mapper.MapToWorkOrderHeader(dto);
            workOrderHeaderAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            workOrderHeaderAdd.CreatedDateTime = DateTime.Now;
            workOrderHeaderAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            workOrderHeaderAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.WorkOrderHeaderRepository.Add(workOrderHeaderAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
                
            _logger.Information("Work Order Header added successfully, Work Order Header Id: {WorkOrderHeaderId}", lastInsertedId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header added successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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

    public async Task<ServiceResponse> AddWorkOrderHeaderWithLines(WorkOrderHeaderDto dto)
    {
        try
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            var currentDateTime = DateTime.Now;
            
            var workOrderHeaderAdd = _mapper.MapToWorkOrderHeader(dto);
            workOrderHeaderAdd.CreatedBy = currentUser;
            workOrderHeaderAdd.CreatedDateTime = currentDateTime;
            workOrderHeaderAdd.ModifiedBy = currentUser;
            workOrderHeaderAdd.ModifiedDateTime = currentDateTime;
            await _unitOfWork.WorkOrderHeaderRepository.Add(workOrderHeaderAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            var workOrderLines = _mapper.MapToListOfWorkOrderLine(dto.WorkOrderLines).ToArray();

            foreach (var workOrderLine in workOrderLines)
            {
                workOrderLine.WorkOrderHeaderId = lastInsertedId;
                workOrderLine.CreatedBy = currentUser;
                workOrderLine.CreatedDateTime = currentDateTime;
                workOrderLine.ModifiedBy = currentUser;
                workOrderLine.ModifiedDateTime = currentDateTime;
            }
            
            await _unitOfWork.WorkOrderLineRepository.BulkAdd(workOrderLines);
                
            _logger.Information("Work Order Header and lines added successfully, Work Order Header Id: {WorkOrderHeaderId}", lastInsertedId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header added successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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

    public async Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
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
            
            record.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            record.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.WorkOrderHeaderRepository.Update(record);
            
            _logger.Information("Work Order Header updated successfully, Work Order Header Id: {WorkOrderHeaderId}", dto.WorkOrderHeaderId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header updated successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
    public async Task<ServiceResponse> DeleteWorkOrderHeader(int id)
    {
        try
        {
            await _unitOfWork.WorkOrderHeaderRepository.Delete(id);
            
            _logger.Information("Work Order Header deleted successfully, Work Order Header Id: {WorkOrderHeaderId}", id);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Header deleted successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
    public async Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderById(int id)
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
    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetAllWorkOrderHeaderPagedList(int pageNumber, int pageSize)
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
                Success = true,
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
    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters)
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
                Success = true,
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
    public async Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        try
        {
            var workOrderLineAdd = _mapper.MapToWorkOrderLine(dto);
            workOrderLineAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            workOrderLineAdd.CreatedDateTime = DateTime.Now;
            workOrderLineAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            workOrderLineAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.WorkOrderLineRepository.Add(workOrderLineAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
                
            _logger.Information("Work Order Line added successfully, Work Order Line Id: {WorkOrderLineId}", lastInsertedId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line added successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
    public async Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        try
        {
            var record = await _unitOfWork.WorkOrderLineRepository.GetById(dto.WorkOrderLineId, true);
            
            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Work Order Line has been modified by another user, please refresh and try again");
            }
            
            record.UpdateProperties(_mapper.MapToWorkOrderLine(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Work Order Line"
                };
            }
            
            record.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            record.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.WorkOrderLineRepository.Update(record);
            
            _logger.Information("Work Order Line updated successfully, Work Order Line Id: {WorkOrderLineId}", dto.WorkOrderLineId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line updated successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
    public async Task<ServiceResponse> DeleteWorkOrderLine(int id)
    {
        try
        {
            await _unitOfWork.ItemRequisitionRepository.Delete(id);
            
            _logger.Information("Work Order Line deleted successfully, Work Order Line Id: {WorkOrderLineId}", id);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "Work Order Line deleted successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
    public async Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLineById(int id)
    {
        try
        {
            var workOrderLine = await _unitOfWork.WorkOrderLineRepository.GetById(id);
            
            return new ServiceResponse<WorkOrderLineDto>
            {
                Success = true,
                Data = _mapper.MapToWorkOrderLineDto(workOrderLine),
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
    public async Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineByWorkOrderHeaderId(int id)
    {
        try
        {
            var workOrderLines = await _unitOfWork.WorkOrderLineRepository.GetByWorkOrderHeaderId(id);
            
            return new ServiceResponse<IEnumerable<WorkOrderLineDto>>
            {
                Data = _mapper.MapToListOfWorkOrderLineDto(workOrderLines),
                Message = "Work Order Lines retrieved successfully",
                Success = true,
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
    public async Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeaderByIdWithLines(int id)
    {
        try
        {
            var workOrderHeader = await _unitOfWork.WorkOrderHeaderRepository.GetByIdWithLines(id);
            
            return new ServiceResponse<WorkOrderHeaderDto>
            {
                Success = true,
                Data = _mapper.MapToWorkOrderHeaderDto(workOrderHeader),
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

    public async Task<ServiceResponse> AddItemRequisition(ItemRequisitionDto dto)
    {
        try
        {
            var itemRequisitionAdd = _mapper.MapToItemRequisition(dto);
            itemRequisitionAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            itemRequisitionAdd.CreatedDateTime = DateTime.Now;
            itemRequisitionAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            itemRequisitionAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.ItemRequisitionRepository.Add(itemRequisitionAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            _logger.Information("Item Requisition added successfully, Item Requisition Id: {ItemRequisitionId}", lastInsertedId);
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition added successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
            
            record.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            record.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.ItemRequisitionRepository.Update(record);
            
            _logger.Information("Item Requisition updated successfully, Item Requisition Id: {ItemRequisitionId}", dto.ItemRequisitionId);
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition updated successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Success = true,
                Message = "Item Requisition deleted successfully",
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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

    public async Task<ServiceResponse<ItemRequisitionDto>> GetItemRequisitionById(int id)
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

    public async Task<ServiceResponse<IEnumerable<ItemRequisitionDto>>> GetItemRequisitionByWorkOrderLineId(int id)
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