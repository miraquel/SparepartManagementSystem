using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class WorkOrderService : IWorkOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();

    public WorkOrderService(IMapper mapper, IUnitOfWork unitOfWork, IGMKSMSServiceGroup gmkSmsServiceGroup, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ServiceResponse> AddWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        try
        {
            await _unitOfWork.WorkOrderHeaderRepository.Add(_mapper.Map<WorkOrderHeader>(dto));
            
            _unitOfWork.Commit();
            
            var lastInsertedId = await _unitOfWork.WorkOrderHeaderRepository.GetLastInsertedId();
                
            _logger.Information("Work Order Header added successfully, Work Order Header Id: {WorkOrderHeaderId}", lastInsertedId);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            await _unitOfWork.WorkOrderHeaderRepository.Update(_mapper.Map<WorkOrderHeader>(dto));
            
            _unitOfWork.Commit();
            
            _logger.Information("Work Order Header updated successfully, Work Order Header Id: {WorkOrderHeaderId}", dto.WorkOrderHeaderId);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            
            _unitOfWork.Commit();
            
            _logger.Information("Work Order Header deleted successfully, Work Order Header Id: {WorkOrderHeaderId}", id);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
                Data = _mapper.Map<WorkOrderHeaderDto>(workOrderHeader)
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var workOrderHeaders = await _unitOfWork.WorkOrderHeaderRepository.GetAllPagedList(pageNumber, pageSize);
            
            _logger.Information("Work Order Headers fetched successfully, Total Count: {TotalCount}", workOrderHeaders);
            
            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Data = _mapper.Map<PagedListDto<WorkOrderHeaderDto>>(workOrderHeaders),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderByParamsPagedList(int pageNumber, int pageSize, WorkOrderHeaderDto entity)
    {
        try
        {
            var workOrderHeaders = await _unitOfWork.WorkOrderHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, _mapper.Map<WorkOrderHeader>(entity));
            
            _logger.Information("Work Order Headers fetched successfully, Total Count: {TotalCount}", workOrderHeaders);
            
            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Data = _mapper.Map<PagedListDto<WorkOrderHeaderDto>>(workOrderHeaders),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            await _unitOfWork.WorkOrderLineRepository.Add(_mapper.Map<WorkOrderLine>(dto));
            
            _unitOfWork.Commit();
            
            var lastInsertedId = await _unitOfWork.WorkOrderLineRepository.GetLastInsertedId();
                
            _logger.Information("Work Order Line added successfully, Work Order Line Id: {WorkOrderLineId}", lastInsertedId);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            await _unitOfWork.WorkOrderLineRepository.Update(_mapper.Map<WorkOrderLine>(dto));
            
            _unitOfWork.Commit();
            
            _logger.Information("Work Order Line updated successfully, Work Order Line Id: {WorkOrderLineId}", dto.WorkOrderLineId);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            await _unitOfWork.WorkOrderLineRepository.Delete(id);
            
            _unitOfWork.Commit();
            
            _logger.Information("Work Order Line deleted successfully, Work Order Line Id: {WorkOrderLineId}", id);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
                Data = _mapper.Map<WorkOrderLineDto>(workOrderLine)
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
                Data = _mapper.Map<IEnumerable<WorkOrderLineDto>>(workOrderLines),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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