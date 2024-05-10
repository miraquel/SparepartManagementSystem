using System.Data.SqlTypes;
using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();

    public GoodsReceiptService(IUnitOfWork unitOfWork, IGMKSMSServiceGroup gmkSmsServiceGroup, IHttpContextAccessor httpContextAccessor, MapperlyMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> AddGoodsReceiptHeader(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var goodsReceiptHeaderAdd = _mapper.MapToGoodsReceiptHeader(dto);
            
            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            goodsReceiptHeaderAdd.CreatedBy = currentUser;
            goodsReceiptHeaderAdd.CreatedDateTime = DateTime.Now;
            goodsReceiptHeaderAdd.ModifiedBy = currentUser;
            goodsReceiptHeaderAdd.ModifiedDateTime = DateTime.Now;
            
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeaderAdd);
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully", lastInsertedId);
            
            _unitOfWork.Commit();

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

    public async Task<ServiceResponse> DeleteGoodsReceiptHeader(int id)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header deleted successfully", id);
            
            _unitOfWork.Commit();

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

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeader()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAll();

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.MapToListOfGoodsReceiptHeaderDto(result),
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

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> GetGoodsReceiptHeaderById(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header retrieved successfully", result.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.MapToGoodsReceiptHeaderDto(result),
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

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetGoodsReceiptHeaderByParams(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParams(_mapper.MapToGoodsReceiptHeader(dto));

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.MapToListOfGoodsReceiptHeaderDto(result),
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

    public async Task<ServiceResponse> UpdateGoodsReceiptHeader(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var oldRecord = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(dto.GoodsReceiptHeaderId, true);

            if (oldRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }
            
            var newRecord = _mapper.MapToGoodsReceiptHeader(dto);
            newRecord.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            newRecord.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(GoodsReceiptHeader.ForUpdate(oldRecord, newRecord));

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully", dto.GoodsReceiptHeaderId);
            
            _unitOfWork.Commit();

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

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeaderPagedList(int pageNumber, int pageSize)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAllPagedList(pageNumber, pageSize);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = new PagedListDto<GoodsReceiptHeaderDto>(
                    _mapper.MapToListOfGoodsReceiptHeaderDto(result.Items),
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

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, _mapper.MapToGoodsReceiptHeader(entity));

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = new PagedListDto<GoodsReceiptHeaderDto>(
                    _mapper.MapToListOfGoodsReceiptHeaderDto(result.Items),
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
    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> GetGoodsReceiptHeaderByIdWithLines(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(id);
            
            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.MapToGoodsReceiptHeaderDto(result),
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
    public async Task<ServiceResponse> AddGoodsReceiptHeaderWithLines(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            var currentDateTime = DateTime.Now;
            
            var goodsReceiptHeaderAdd = _mapper.MapToGoodsReceiptHeader(dto);
            goodsReceiptHeaderAdd.CreatedBy = currentUser;
            goodsReceiptHeaderAdd.CreatedDateTime = currentDateTime;
            goodsReceiptHeaderAdd.ModifiedBy = currentUser;
            goodsReceiptHeaderAdd.ModifiedDateTime = currentDateTime;
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeaderAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            var goodsReceiptLinesAdd = _mapper.MapToListOfGoodsReceiptLine(dto.GoodsReceiptLines).ToArray();
            foreach (var goodsReceiptLineAdd in goodsReceiptLinesAdd)
            {
                goodsReceiptLineAdd.GoodsReceiptHeaderId = lastInsertedId;
                goodsReceiptLineAdd.CreatedBy = currentUser;
                goodsReceiptLineAdd.CreatedDateTime = currentDateTime;
                goodsReceiptLineAdd.ModifiedBy = currentUser;
                goodsReceiptLineAdd.ModifiedDateTime = currentDateTime;
            }
            
            await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(goodsReceiptLinesAdd);
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully with {lines} lines inserted", lastInsertedId, goodsReceiptLinesAdd.Length);
            
            _unitOfWork.Commit();

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
    public async Task<ServiceResponse> UpdateGoodsReceiptHeaderWithLines(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            var currentDateTime = DateTime.Now;
            
            var oldHeaderRecord = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(dto.GoodsReceiptHeaderId, true);

            if (oldHeaderRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }
            
            var newHeaderRecord = _mapper.MapToGoodsReceiptHeader(dto);
            newHeaderRecord.ModifiedBy = currentUser;
            newHeaderRecord.ModifiedDateTime = currentDateTime;
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(GoodsReceiptHeader.ForUpdate(oldHeaderRecord, newHeaderRecord));
            
            var goodsReceiptLines = await _unitOfWork.GoodsReceiptLineRepository.GetByGoodsReceiptHeaderId(dto.GoodsReceiptHeaderId);
                
            // if the line is exists in the database, but not exists in the dto, then delete the line
            var receiptLines = goodsReceiptLines as GoodsReceiptLine[] ?? goodsReceiptLines.ToArray();
            foreach (var goodsReceiptLine in receiptLines)
            {
                if (dto.GoodsReceiptLines.All(x => x.GoodsReceiptLineId != goodsReceiptLine.GoodsReceiptLineId))
                {
                    await _unitOfWork.GoodsReceiptLineRepository.Delete(goodsReceiptLine.GoodsReceiptLineId);
                }
            }
            
            var goodsReceiptLinesDto = dto.GoodsReceiptLines as GoodsReceiptLineDto[] ?? dto.GoodsReceiptLines.ToArray();
            foreach (var goodsReceiptLineDto in goodsReceiptLinesDto)
            {
                var oldRecord = receiptLines.FirstOrDefault(x => x.GoodsReceiptLineId == goodsReceiptLineDto.GoodsReceiptLineId);
                
                // if the line is exists in the database, then update the line, otherwise add the line
                if (oldRecord is null)
                {
                    var goodsReceiptLineAdd = _mapper.MapToGoodsReceiptLine(goodsReceiptLineDto);
                    goodsReceiptLineAdd.CreatedBy = currentUser;
                    goodsReceiptLineAdd.CreatedDateTime = currentDateTime;
                    goodsReceiptLineAdd.ModifiedBy = currentUser;
                    goodsReceiptLineAdd.ModifiedDateTime = currentDateTime;
                    await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLineAdd);
                }
                else
                {
                    if (oldRecord.ModifiedDateTime > goodsReceiptLineDto.ModifiedDateTime)
                    {
                        throw new Exception("Goods Receipt Line has been modified by another user, please refresh the page and try again");
                    }
                    
                    var newRecord = _mapper.MapToGoodsReceiptLine(goodsReceiptLineDto);
                    if (GoodsReceiptLine.Compare(oldRecord, newRecord)) continue;
                    newRecord.ModifiedBy = currentUser;
                    newRecord.ModifiedDateTime = currentDateTime;
                    await _unitOfWork.GoodsReceiptLineRepository.Update(GoodsReceiptLine.ForUpdate(oldRecord, newRecord));
                }
            }
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully with {lines} lines updated", dto.GoodsReceiptHeaderId, dto.GoodsReceiptLines.Count);
            
            _unitOfWork.Commit();

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
            var oldRecord = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(dto.GoodsReceiptHeaderId, true);

            if (oldRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }

            if (oldRecord.IsSubmitted is true)
            {
                throw new Exception("Goods Receipt Header has been posted to AX");
            }

            if (oldRecord.GoodsReceiptLines.Count == 0)
            {
                throw new Exception("Goods Receipt Line is not exists, at least must be 1 line exists to post the Goods Receipt Header to AX");
            }

            if (string.IsNullOrEmpty(oldRecord.PackingSlipId))
            {
                throw new Exception("Packing Slip Id is required to post the Goods Receipt Header to AX");
            }

            if (oldRecord.TransDate.Equals(SqlDateTime.MinValue.Value))
            {
                throw new Exception("Receipt Date is required to post the Goods Receipt Header to AX");
            }

            if (oldRecord.GoodsReceiptLines.Any(x => x.ReceiveNow == 0))
            {
                throw new Exception("Receive Now must be greater than 0 to post the Goods Receipt Header to AX");
            }
            
            // check if all goods receipt line with type item has an inventLocationId, if not then throw an exception
            if (oldRecord.GoodsReceiptLines.Where(x => x.ProductType == ProductType.Item).Any(x => string.IsNullOrEmpty(x.InventLocationId)))
            {
                throw new Exception("All goods receipt line with type item must have an InventLocationId to post the Goods Receipt Header to AX");
            }
            
            // check if all goods receipt line with type item has the same InventLocationId, if not then throw an exception
            if (oldRecord.GoodsReceiptLines.Where(x => x.ProductType == ProductType.Item).Select(x => x.InventLocationId).Distinct().Count() > 1)
            {
                throw new Exception("All goods receipt line with type item must have the same InventLocationId to post the Goods Receipt Header to AX");
            }

            var newRecord = oldRecord.Clone();
            newRecord.IsSubmitted = true;
            newRecord.SubmittedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            newRecord.SubmittedDate = DateTime.Now;
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(GoodsReceiptHeader.ForUpdate(oldRecord, newRecord));
            
            var result = await _gmkSmsServiceGroup.PostPurchPackingSlip(dto);
            
            if (!result.Success)
                throw new Exception(result.ErrorMessages?.FirstOrDefault() ?? "Error when posting to AX");

            _logger.Information("Goods Receipt Header posted to Ax successfully, Message: {Message}", result.Message);
            
            _unitOfWork.Commit();

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