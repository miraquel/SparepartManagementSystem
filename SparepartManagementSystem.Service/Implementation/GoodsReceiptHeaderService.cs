using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptHeaderService : IGoodsReceiptHeaderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptHeaderService>();

    public GoodsReceiptHeaderService(IMapper mapper, IUnitOfWork unitOfWork, IGMKSMSServiceGroup gmkSmsServiceGroup, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Add(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Add(_mapper.Map<GoodsReceiptHeader>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully", result.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line added successfully",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Delete(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header deleted successfully", result.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line deleted successfully",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetAll()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAll();

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptHeaderDto>>(result),
                Message = "Journal Line retrieved successfully",
                Success = true
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

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header retrieved successfully", result.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line added successfully",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetByParams(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParams(_mapper.Map<GoodsReceiptHeader>(dto));

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptHeaderDto>>(result),
                Message = "Journal Line retrieved successfully",
                Success = true
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

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Update(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully", result.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line updated successfully",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllPagedList(int pageNumber, int pageSize)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAllPagedList(pageNumber, pageSize);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<PagedListDto<GoodsReceiptHeaderDto>>(result),
                Message = "Goods Receipt Header retrieved successfully",
                Success = true
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

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, _mapper.Map<GoodsReceiptHeader>(entity));

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = new PagedListDto<GoodsReceiptHeaderDto>(
                    _mapper.Map<IEnumerable<GoodsReceiptHeaderDto>>(result.Items),
                    result.PageNumber,
                    result.PageSize,
                    result.TotalCount),
                Message = "Goods Receipt Header retrieved successfully",
                Success = true
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

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> GetByIdWithLines(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(id);
            
            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Goods Receipt Header retrieved successfully",
                Success = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> AddWithLines(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var header = await _unitOfWork.GoodsReceiptHeaderRepository.Add(_mapper.Map<GoodsReceiptHeader>(dto));
            var lines = _mapper.Map<IEnumerable<GoodsReceiptLine>>(dto.GoodsReceiptLines).ToArray();
            foreach (var line in lines)
            {
                line.GoodsReceiptHeaderId = header.GoodsReceiptHeaderId;
            }
            var linesResult = await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(lines);
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(header.GoodsReceiptHeaderId);
            
            _unitOfWork.Commit();
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully with {lines} lines inserted", result.GoodsReceiptHeaderId, linesResult);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line added successfully",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> PostToAx(GoodsReceiptHeaderDto dto)
    {
        try
        {
            if (dto.GoodsReceiptLines is null || !dto.GoodsReceiptLines.Any())
                throw new Exception("Goods Receipt Lines is empty");
            
            var updateDto = dto.Merge(new GoodsReceiptHeaderDto
            {
                IsSubmitted = true,
                SubmittedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System",
                SubmittedDate = DateTime.Now
            });
            
            var updateResult = await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(updateDto));
            
            var result = await _gmkSmsServiceGroup.PostPurchPackingSlip(dto);
            
            if (!result.Success)
                throw new Exception(result.ErrorMessages?.FirstOrDefault() ?? "Error when posting to AX");

            _unitOfWork.Commit();

            _logger.Information("Goods Receipt Header posted to Ax successfully, Message: {Message}", result.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(updateResult),
                Message = "Goods Receipt Header has been successfully posted to AX",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}