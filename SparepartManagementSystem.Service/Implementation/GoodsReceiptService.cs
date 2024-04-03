using System.Data.SqlTypes;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();

    public GoodsReceiptService(IMapper mapper, IUnitOfWork unitOfWork, IGMKSMSServiceGroup gmkSmsServiceGroup, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse> Add(GoodsReceiptHeaderDto dto)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(_mapper.Map<GoodsReceiptHeader>(dto));
            var lastInsertedId = await _unitOfWork.GoodsReceiptHeaderRepository.GetLastInsertedId();

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully", lastInsertedId);

            return new ServiceResponse
            {
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

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> Delete(int id)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header deleted successfully", id);

            return new ServiceResponse
            {
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> Update(GoodsReceiptHeaderDto dto)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully", dto.GoodsReceiptHeaderId);

            return new ServiceResponse
            {
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

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<int>> GetLastInsertedId()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetLastInsertedId();
            
            _logger.Information("Goods receipt last inserted id retrieved successfully, id: {GoodsReceiptHeaderId}", result);

            return new ServiceResponse<int>
            {
                Data = result,
                Message = "Goods receipt last inserted id retrieved successfully",
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

            return new ServiceResponse<int>
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
    public async Task<ServiceResponse> AddWithLines(GoodsReceiptHeaderDto dto)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(_mapper.Map<GoodsReceiptHeader>(dto));
            var lastInsertedId = await _unitOfWork.GoodsReceiptHeaderRepository.GetLastInsertedId();
            
            var lines = _mapper.Map<IEnumerable<GoodsReceiptLine>>(dto.GoodsReceiptLines).ToArray();
            foreach (var line in lines)
            {
                line.GoodsReceiptHeaderId = lastInsertedId;
            }
            var linesCount = await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(lines);
            
            _unitOfWork.Commit();
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully with {lines} lines inserted", lastInsertedId, linesCount);

            return new ServiceResponse
            {
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

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse> UpdateWithLines(GoodsReceiptHeaderDto dto)
    {
        try
        {
            // update the header
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(dto));

            if (dto.GoodsReceiptLines.Any())
            {
                var goodsReceiptLines = await _unitOfWork.GoodsReceiptLineRepository.GetByGoodsReceiptHeaderId(dto.GoodsReceiptHeaderId);
                
                // if the line is exists in the database, but not exists in the dto, then delete the line
                foreach (var goodsReceiptLine in goodsReceiptLines)
                {
                    if (dto.GoodsReceiptLines.All(x => x.GoodsReceiptLineId != goodsReceiptLine.GoodsReceiptLineId))
                    {
                        await _unitOfWork.GoodsReceiptLineRepository.Delete(goodsReceiptLine.GoodsReceiptLineId);
                    }
                }
                
                var goodsReceiptLinesDto = dto.GoodsReceiptLines as GoodsReceiptLineDto[] ?? dto.GoodsReceiptLines.ToArray();
                foreach (var goodsReceiptLineDto in goodsReceiptLinesDto)
                {
                    // if the line is exists in the database, then update the line, otherwise add the line
                    if (goodsReceiptLineDto.GoodsReceiptLineId == 0)
                    {
                        await _unitOfWork.GoodsReceiptLineRepository.Add(_mapper.Map<GoodsReceiptLine>(goodsReceiptLineDto));
                    }
                    else
                    {
                        await _unitOfWork.GoodsReceiptLineRepository.Update(_mapper.Map<GoodsReceiptLine>(goodsReceiptLineDto));
                    }
                }
            }
            
            _unitOfWork.Commit();
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully with {lines} lines updated", dto.GoodsReceiptHeaderId, dto.GoodsReceiptLines.Count);

            return new ServiceResponse
            {
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

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse> PostToAx(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var beforePost = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(dto.GoodsReceiptHeaderId);
            
            var beforePostDto = _mapper.Map<GoodsReceiptHeaderDto>(beforePost);
            if (beforePostDto.IsSubmitted is true)
                throw new Exception("Goods Receipt Header has been posted to AX");
            
            if (!beforePostDto.GoodsReceiptLines.Any())
                throw new Exception("Goods Receipt Line is not exists, at least must be 1 line exists to post the Goods Receipt Header to AX");
            
            if (string.IsNullOrEmpty(beforePostDto.PackingSlipId))
                throw new Exception("Packing Slip Id is required to post the Goods Receipt Header to AX");
            
            if (beforePostDto.TransDate.Equals(SqlDateTime.MinValue.Value))
                throw new Exception("Receipt Date is required to post the Goods Receipt Header to AX");
            
            var updateHeaderDto = beforePostDto.Merge(new GoodsReceiptHeaderDto
            {
                IsSubmitted = true,
                SubmittedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "System",
                SubmittedDate = DateTime.Now
            });
            
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(updateHeaderDto));
            
            var result = await _gmkSmsServiceGroup.PostPurchPackingSlip(dto);
            
            if (!result.Success)
                throw new Exception(result.ErrorMessages?.FirstOrDefault() ?? "Error when posting to AX");

            _unitOfWork.Commit();

            _logger.Information("Goods Receipt Header posted to Ax successfully, Message: {Message}", result.Message);

            return new ServiceResponse
            {
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