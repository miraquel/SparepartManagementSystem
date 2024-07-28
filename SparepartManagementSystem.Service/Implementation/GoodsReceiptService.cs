using System.Data.SqlTypes;
using MySqlConnector;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;
    private readonly UserClaimDto _userClaim;
    private readonly DateTime _currentDateTime = DateTime.Now;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();

    public GoodsReceiptService(IUnitOfWork unitOfWork, IGMKSMSServiceGroup gmkSmsServiceGroup, MapperlyMapper mapper, UserClaimDto userClaim, RepositoryEvents repositoryEvents)
    {
        _unitOfWork = unitOfWork;
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
        _mapper = mapper;
        _userClaim = userClaim;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> AddGoodsReceiptHeader(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var goodsReceiptHeaderAdd = _mapper.MapToGoodsReceiptHeader(dto);
            
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeaderAdd, _repositoryEvents.OnBeforeAdd);
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully", lastInsertedId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods Receipt Header added successfully",
                Success = true
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

    public async Task<ServiceResponse> DeleteGoodsReceiptHeader(int id)
    {
        try
        {
            await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header deleted successfully", id);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods Receipt Header deleted successfully",
                Success = true
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

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetAllGoodsReceiptHeader()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAll();

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.MapToListOfGoodsReceiptHeaderDto(result),
                Message = "Goods Receipt Headers retrieved successfully",
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetGoodsReceiptHeaderByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParams(parameters);

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.MapToListOfGoodsReceiptHeaderDto(result),
                Message = "Goods Receipt Headers retrieved successfully",
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
            var record = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(dto.GoodsReceiptHeaderId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }

            record.UpdateProperties(_mapper.MapToGoodsReceiptHeader(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Goods Receipt Header"
                };
            }

            await _unitOfWork.GoodsReceiptHeaderRepository.Update(record, _repositoryEvents.OnBeforeUpdate);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully", dto.GoodsReceiptHeaderId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods Receipt Header updated successfully",
                Success = true
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, Dictionary<string, string> parameters)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, parameters);

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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

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
        List<string> errorMessages = [];
        
        try
        {
            var goodsReceiptHeaderAdd = _mapper.MapToGoodsReceiptHeader(dto);
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeaderAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            var goodsReceiptLinesAdd = _mapper.MapToListOfGoodsReceiptLine(dto.GoodsReceiptLines).ToArray();

            _repositoryEvents.OnBeforeAdd += (_, args) =>
            {
                if (args.Entity is not GoodsReceiptLine goodsReceiptLine) return;
                
                goodsReceiptLine.GoodsReceiptHeaderId = lastInsertedId;
            };

            await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(goodsReceiptLinesAdd, InfoMessageEventHandler, onBeforeAdd: _repositoryEvents.OnBeforeAdd);
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully with {lines} lines inserted", lastInsertedId, goodsReceiptLinesAdd.Length);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods receipt header added successfully",
                Success = true
            };

            void InfoMessageEventHandler(object _, object args)
            {
                if (args is not MySqlInfoMessageEventArgs mySqlInfoMessageEventArgs) return;
                
                foreach (var error in mySqlInfoMessageEventArgs.Errors)
                {
                    errorMessages.Add(error.Message);
                    _logger.Error(error.Message);
                }
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            errorMessages.Add(ex.Message);

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

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> AddAndReturnGoodsReceiptHeaderWithLines(GoodsReceiptHeaderDto dto)
    {
        List<string> errorMessages = [];
        
        try
        {
            var goodsReceiptHeaderAdd = _mapper.MapToGoodsReceiptHeader(dto);
            await _unitOfWork.GoodsReceiptHeaderRepository.Add(goodsReceiptHeaderAdd, _repositoryEvents.OnBeforeAdd);

            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            var goodsReceiptLinesAdd = _mapper.MapToListOfGoodsReceiptLine(dto.GoodsReceiptLines).ToArray();

            _repositoryEvents.OnBeforeAdd += (_, args) =>
            {
                if (args.Entity is not GoodsReceiptLine goodsReceiptLine) return;
                
                goodsReceiptLine.GoodsReceiptHeaderId = lastInsertedId;
            };

            await _unitOfWork.GoodsReceiptLineRepository.BulkAdd(goodsReceiptLinesAdd, InfoMessageEventHandler, onBeforeAdd: _repositoryEvents.OnBeforeAdd);
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully with {lines} lines inserted", lastInsertedId, goodsReceiptLinesAdd.Length);
            
            var goodsReceiptHeaderResult = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(lastInsertedId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.MapToGoodsReceiptHeaderDto(goodsReceiptHeaderResult),
                Message = "Goods receipt header added successfully",
                Success = true
            };

            void InfoMessageEventHandler(object _, object args)
            {
                if (args is not MySqlInfoMessageEventArgs mySqlInfoMessageEventArgs) return;
                
                foreach (var error in mySqlInfoMessageEventArgs.Errors)
                {
                    errorMessages.Add(error.Message);
                    _logger.Error(error.Message);
                }
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            errorMessages.Add(ex.Message);

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
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
            var headerRecord = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(dto.GoodsReceiptHeaderId, true);

            if (headerRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }
            
            if (headerRecord.IsSubmitted)
            {
                throw new Exception("Goods Receipt Header has been posted to AX, cannot be modified");
            }

            headerRecord.UpdateProperties(_mapper.MapToGoodsReceiptHeader(dto));
            
            if (headerRecord.IsChanged)
            {
                await _unitOfWork.GoodsReceiptHeaderRepository.Update(headerRecord, _repositoryEvents.OnBeforeUpdate);
            }
                
            // if the line exists in the database, but not exists in the dto, then delete the line
            var goodsReceiptLines = headerRecord.GoodsReceiptLines as GoodsReceiptLine[] ?? headerRecord.GoodsReceiptLines.ToArray();
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
                var lineRecord = goodsReceiptLines.FirstOrDefault(x => x.GoodsReceiptLineId == goodsReceiptLineDto.GoodsReceiptLineId);
                
                // if the line exists in the database, then update the line, otherwise add the line
                if (lineRecord is null)
                {
                    var goodsReceiptLineAdd = _mapper.MapToGoodsReceiptLine(goodsReceiptLineDto);
                    await _unitOfWork.GoodsReceiptLineRepository.Add(goodsReceiptLineAdd, _repositoryEvents.OnBeforeAdd);
                }
                else
                {
                    if (lineRecord.ModifiedDateTime > goodsReceiptLineDto.ModifiedDateTime)
                    {
                        throw new Exception("Goods Receipt Line has been modified by another user, please refresh the page and try again");
                    }
                    
                    lineRecord.UpdateProperties(_mapper.MapToGoodsReceiptLine(goodsReceiptLineDto));
                    
                    if (!lineRecord.IsChanged)
                    {
                        continue;
                    }
                    
                    await _unitOfWork.GoodsReceiptLineRepository.Update(lineRecord, _repositoryEvents.OnBeforeUpdate);
                }
            }
            
            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header and Lines updated successfully with {lines} lines updated", dto.GoodsReceiptHeaderId, dto.GoodsReceiptLines.Count);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods Receipt Header and Lines updated successfully",
                Success = true
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
    public async Task<ServiceResponse> PostToAx(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var record = await _unitOfWork.GoodsReceiptHeaderRepository.GetByIdWithLines(dto.GoodsReceiptHeaderId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Goods Receipt Header has been modified by another user, please refresh the page and try again");
            }

            if (record.IsSubmitted)
            {
                throw new Exception("Goods Receipt Header has been posted to AX");
            }

            if (record.GoodsReceiptLines.Count == 0)
            {
                throw new Exception("Goods Receipt Line is not exists, at least must be 1 line exists to post the Goods Receipt Header to AX");
            }

            if (string.IsNullOrEmpty(record.PackingSlipId))
            {
                throw new Exception("Packing Slip Id is required to post the Goods Receipt Header to AX");
            }

            if (record.TransDate.Equals(SqlDateTime.MinValue.Value))
            {
                throw new Exception("Receipt Date is required to post the Goods Receipt Header to AX");
            }

            if (record.GoodsReceiptLines.Any(x => x.ReceiveNow <= 0))
            {
                throw new Exception("Receive Now must be greater than 0 to post the Goods Receipt Header to AX");
            }
            
            // check if all goods receipt line with type item has an inventLocationId, if not then throw an exception
            if (record.GoodsReceiptLines.Where(x => x.ProductType == ProductType.Item).Any(x => string.IsNullOrEmpty(x.InventLocationId)))
            {
                throw new Exception("All goods receipt line with type item must have an InventLocationId to post the Goods Receipt Header to AX");
            }
            
            // check if all goods receipt line with type item has the same InventLocationId, if not then throw an exception
            if (record.GoodsReceiptLines.Where(x => x.ProductType == ProductType.Item).Select(x => x.InventLocationId).Distinct().Count() > 1)
            {
                throw new Exception("All goods receipt line with type item must have the same InventLocationId to post the Goods Receipt Header to AX");
            }
            
            record.IsSubmitted = true;
            record.SubmitStatus = SubmitStatus.Submitted;
            record.SubmittedDate = _currentDateTime;
            record.SubmittedBy = _userClaim.Username;
            await _unitOfWork.GoodsReceiptHeaderRepository.Update(record, _repositoryEvents.OnBeforeUpdate);
            
            var result = await _gmkSmsServiceGroup.PostPurchPackingSlip(dto);
            
            if (!result.Success)
            {
                throw new Exception(result.ErrorMessages?.FirstOrDefault() ?? "Error when posting to AX");
            }

            _logger.Information("Goods Receipt Header posted to Ax successfully, Message: {Message}", result.Message);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Goods Receipt Header has been successfully posted to AX",
                Success = true
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

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<string>> GetGoodsReceiptLabelTemplate(GoodsReceiptLineDto dto, int copies = 1)
    {
        try
        {
            var goodsReceiptHeader = await _unitOfWork.GoodsReceiptLineRepository.GetByIdWithGoodsReceiptHeader(dto.GoodsReceiptLineId);
            var goodsReceiptLine = goodsReceiptHeader.GoodsReceiptLines.SingleOrDefault() ?? throw new Exception("Goods Receipt Line not found");
            
            var response = await _gmkSmsServiceGroup.GetVendPackingSlipJourWithLines(new VendPackingSlipJourDto
            {
                PurchId = goodsReceiptHeader.PurchId,
                PackingSlipId = goodsReceiptHeader.PackingSlipId,
                DeliveryDate = goodsReceiptHeader.TransDate
            });
            var vendPackingSlipJour = response.Data ?? throw new Exception(response.ErrorMessages?.FirstOrDefault() ?? "Error when retrieving VendPackingSlipJour");
            var item = vendPackingSlipJour.VendPackingSlipTrans.FirstOrDefault(x => x.ItemId == goodsReceiptLine.ItemId) ?? throw new Exception("Item not found in VendPackingSlipTrans");
            
            List<string> itemName = [];
            if (item is { ItemName.Length: > 44 })
            {
                var firstLine = item.ItemName[..44].Trim();
                itemName.Add($"TEXT 0, 5, \"1\", 0, 1, 2, \"{firstLine}\"");
                
                var secondLine = item.ItemName[firstLine.Length..].Trim();
                itemName.Add($"TEXT 0, 55, \"0\", 0, 1, 2, \"{(secondLine.Length > 44 ? secondLine[..44] : secondLine)}\"");
            }
            else
            {
                var firstLine = item.ItemName.Trim();
                // replace \" with \["]
                firstLine = firstLine.Replace("\"", "\\[\"]");
                itemName.Add($"TEXT 0, 25, \"1\", 0, 1, 2, \"{firstLine}\"");
            }

            string[] printData =
            [
                "SIZE 72 mm,30 mm",
                "CLS",
                "CODEPAGE 850",
                ..itemName,
                $"TEXT 0, 110, \"1\", 0, 1, 1, \"Item Id: {item.ItemId}\"",
                $"TEXT 0, 150, \"1\", 0, 1, 1, \"GR No: {vendPackingSlipJour.InternalPackingSlipId}\"",
                $"TEXT 0, 190, \"1\", 0, 1, 1, \"GR Date: {vendPackingSlipJour.DeliveryDate:dd-MMM-yyyy}\"",
                $"QRCODE 320,110,H,3,A,0,\"http://www.gmk.id/{item.ItemId}/\"",
                $"PRINT 1, {copies}",
                "END",
            ];
            
            return new ServiceResponse<string>
            {
                Data = string.Join("\r\n", printData),
                Message = "Goods Receipt Label Template retrieved successfully",
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

            return new ServiceResponse<string>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}