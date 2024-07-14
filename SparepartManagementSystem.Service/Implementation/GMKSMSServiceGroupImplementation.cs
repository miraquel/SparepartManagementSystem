using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Caching.Distributed;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Domain.Extensions;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;
using SparepartManagementSystem.Shared.Extensions;

namespace SparepartManagementSystem.Service.Implementation;

public class GMKSMSServiceGroupImplementation : IGMKSMSServiceGroup
{
    private readonly GMKSMSService _client;
    private readonly CallContext _context;
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserClaimDto _userClaim;
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;

    public GMKSMSServiceGroupImplementation(GMKSMSService client, CallContext context, MapperlyMapper mapper, IUnitOfWork unitOfWork, UserClaimDto userClaim, IDistributedCache distributedCache, DistributedCacheEntryOptions cacheEntryOptions)
    {
        _client = client;
        _context = context;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userClaim = userClaim;
        _distributedCache = distributedCache;
        _cacheEntryOptions = cacheEntryOptions;
    }

    public async Task<ServiceResponse<InventTableDto>> GetInventTable(string itemId)
    {
        try
        {
            if (_distributedCache.TryGetValue(itemId, out InventTableDto? inventTableDto))
            {
                return new ServiceResponse<InventTableDto>
                {
                    Data = inventTableDto,
                    Message = "Invent Table retrieved successfully",
                    Success = true
                };
            }
            
            if (_userClaim.UserId > 0)
            {
                var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(_userClaim.UserId);
                var filters = rowLevelAccesses.FilterRowLevelAccess(itemId, AxTable.InventTable).ToArray();
                if (filters.Length == 0)
                {
                    throw new Exception("You do not have access to this item");
                }
            }

            var request = new GMKSMSServiceGetInventTableRequest
            {
                itemId = itemId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = _client.getInventTableAsync(request).Result;
            
            var inventTablesDto = _mapper.MapToInventTableDto(response.response);
            
            await _distributedCache.SetAsync(itemId, inventTablesDto, _cacheEntryOptions);

            return new ServiceResponse<InventTableDto>
            {
                Data = inventTablesDto,
                Message = "Invent Table retrieved successfully",
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

            return new ServiceResponse<InventTableDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<PagedListDto<InventTableDto>>> GetInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventTablePagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                productName = dto.ProductName,
                searchName = dto.SearchName,
                itemId = dto.ItemId
            };
            
            if (_userClaim.UserId > 0)
            {
                var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(_userClaim.UserId);
                request.itemIdFilters = rowLevelAccesses.FilterRowLevelAccess(dto.ItemId, AxTable.InventTable).ToArray();
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventTablePagedListAsync(request);

            var inventTablePagedList = new PagedListDto<InventTableDto>(
                _mapper.MapToListOfInventTableDto(response.response.Items.Select(x =>
                {
                    var isValidUri = Uri.TryCreate(x.Image, UriKind.Absolute, out var result);
                    var isValidMimeType = isValidUri && MimeTypes.TryGetMimeType(result?.LocalPath, out var mimeType) && mimeType.StartsWith("image");

                    x.Image = isValidMimeType ? WebUtility.UrlEncode(x.Image) : string.Empty;
                    return x;
                })),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<InventTableDto>>
            {
                Data = inventTablePagedList,
                Message = "Invent Table Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<InventTableDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }
    
    public async Task<ServiceResponse<PagedListDto<InventTableDto>>> GetRawInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventTablePagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                productName = dto.ProductName,
                searchName = dto.SearchName,
                itemId = dto.ItemId,
            };
            
            if (_userClaim.UserId > 0)
            {
                var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(_userClaim.UserId);
                request.itemIdFilters = rowLevelAccesses.FilterRowLevelAccess(dto.ItemId, AxTable.InventTable).ToArray();
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventTablePagedListAsync(request);

            var inventTablePagedList = new PagedListDto<InventTableDto>(
                _mapper.MapToListOfInventTableDto(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<InventTableDto>>
            {
                Data = inventTablePagedList,
                Message = "Invent Table Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<InventTableDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<PagedListDto<PurchTableDto>>> GetPurchTablePagedList(int pageNumber, int pageSize, PurchTableSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetPurchTablePagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                parm = new GMKPurchTableDataContract
                {
                    PurchId = dto.PurchId,
                    PurchName = dto.PurchName,
                    OrderAccount = dto.OrderAccount,
                    InvoiceAccount = dto.InvoiceAccount
                }
            };
            
            if (_userClaim.UserId > 0)
            {
                var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(_userClaim.UserId);
                request.filters = rowLevelAccesses.FilterRowLevelAccess(dto.OrderAccount, AxTable.PurchTable).ToArray();
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getPurchTablePagedListAsync(request);

            var purchTablePagedList = new PagedListDto<PurchTableDto>(
                _mapper.MapToListOfPurchTableDto(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<PurchTableDto>>
            {
                Data = purchTablePagedList,
                Message = "Purch Table Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<PurchTableDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }
    public async Task<ServiceResponse<IEnumerable<PurchLineDto>>> GetPurchLineList(string purchId)
    {
        try
        {
            var request = new GMKSMSServiceGetPurchLineListRequest
            {
                purchId = purchId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getPurchLineListAsync(request);

            return new ServiceResponse<IEnumerable<PurchLineDto>>
            {
                Data = _mapper.MapToListOfPurchLineDto(response.response),
                Message = "Purch Line List retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PurchLineDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }
    public async Task<ServiceResponse<PagedListDto<WMSLocationDto>>> GetWMSLocationPagedList(int pageNumber, int pageSize, WMSLocationSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWMSLocationPagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                parm = new GMKWMSLocationDataContract
                {
                    WMSLocationId = dto.WMSLocationId
                }
            };
            
            if (_userClaim.UserId > 0)
            {
                var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetByUserId(_userClaim.UserId);
                request.filters = userWarehouses.FilterByParm(dto.InventLocationId).ToArray();
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWMSLocationPagedListAsync(request);

            var wmsLocationPagedList = new PagedListDto<WMSLocationDto>(
                _mapper.MapToListOfWMSLocationDto(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<WMSLocationDto>>
            {
                Data = wmsLocationPagedList,
                Message = "WMS Location Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<WMSLocationDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<WMSLocationDto>> GetWMSLocation(WMSLocationDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWMSLocationRequest
            {
                parm = _mapper.MapToGMKWMSLocationDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWMSLocationAsync(request);

            return new ServiceResponse<WMSLocationDto>
            {
                Data = _mapper.MapToWMSLocationDto(response.response),
                Message = "WMS Location retrieved successfully",
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

            return new ServiceResponse<WMSLocationDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<GMKServiceResponseDto>> PostPurchPackingSlip(GoodsReceiptHeaderDto dto)
    {
        try 
        {
            var lines = dto.GoodsReceiptLines.Select(line => new GMKPurchParmLineDataContract
                {
                    PurchaseLineLineNumber = line.LineNumber,
                    ReceiveNow = line.ReceiveNow,
                    InventLocationId = line.InventLocationId,
                    WMSLocationId = line.WMSLocationId
                })
                .ToArray();
            var request = new GMKSMSServicePostPurchPackingSlipRequest
            {
                request = new GMKPurchParmTableDataContract
                {
                    PurchId = dto.PurchId,
                    Num = dto.PackingSlipId,
                    Description = dto.Description,
                    TransDate = dto.TransDate,
                    PurchParmLines = lines
                }
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.postPurchPackingSlipAsync(request);
            
            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }

            return new ServiceResponse<GMKServiceResponseDto>
            {
                Data = _mapper.MapToGMKServiceResponseDto(response.response),
                Message = "Purch Packing Slip posted successfully",
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

            return new ServiceResponse<GMKServiceResponseDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }
    public async Task<ServiceResponse<PurchTableDto>> GetPurchTable(string purchId)
    {
        try
        {
            var request = new GMKSMSServiceGetPurchTableRequest
            {
                purchId = purchId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getPurchTableAsync(request);

            return new ServiceResponse<PurchTableDto>
            {
                Data = _mapper.MapToPurchTableDto(response.response),
                Message = "Purch Table retrieved successfully",
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

            return new ServiceResponse<PurchTableDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }
    public async Task<ServiceResponse<IEnumerable<InventSumDto>>> GetInventSumList(InventSumSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventSumListRequest
            {
                itemId = dto.ItemId,
                inventLocationId = dto.InventLocationId,
                wMSLocationId = dto.WMSLocationId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventSumListAsync(request);

            return new ServiceResponse<IEnumerable<InventSumDto>>
            {
                Data = _mapper.MapToListOfInventSumDto(response.response),
                Message = "Invent Sum List retrieved successfully",
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

            return new ServiceResponse<IEnumerable<InventSumDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderPagedListV2(int pageNumber, int pageSize, WorkOrderHeaderDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderPagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                parm = _mapper.MapToGMKWorkOrderDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderPagedListAsync(request);

            var workOrderPagedList = new PagedListDto<WorkOrderHeaderDto>(
                _mapper.MapToListOfWorkOrderHeaderDto(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Data = workOrderPagedList,
                Message = "Work Order Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineListV2(string workOrderHeaderId)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderLineListRequest
            {
                agsEAMWOID = workOrderHeaderId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderLineListAsync(request);

            return new ServiceResponse<IEnumerable<WorkOrderLineDto>>
            {
                Data = _mapper.MapToListOfWorkOrderLineDto(response.response),
                Message = "Work Order Line List retrieved successfully",
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

            return new ServiceResponse<IEnumerable<WorkOrderLineDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<IEnumerable<DimensionDto>>> GetDimensionList(string dimensionName)
    {
        try
        {
            if (_distributedCache.TryGetValue(dimensionName, out IEnumerable<DimensionDto>? dimensions))
            {
                return new ServiceResponse<IEnumerable<DimensionDto>>
                {
                    Data = dimensions,
                    Message = "Dimension List retrieved successfully",
                    Success = true
                };
            }
            
            var request = new GMKSMSServiceGetDimensionListRequest
            {
                dimensionName = dimensionName
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getDimensionListAsync(request);
            
            var dimensionsDto = _mapper.MapToListOfDimensionDto(response.response);
            var dimensionsDtoArray = dimensionsDto as DimensionDto[] ?? dimensionsDto.ToArray();

            await _distributedCache.SetAsync(dimensionName, dimensionsDtoArray, _cacheEntryOptions);

            return new ServiceResponse<IEnumerable<DimensionDto>>
            {
                Data = dimensionsDtoArray,
                Message = "Dimension List retrieved successfully",
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

            return new ServiceResponse<IEnumerable<DimensionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<VendPackingSlipJourDto>> GetVendPackingSlipJourWithLines(string packingSlipId)
    {
        try
        {
            var request = new GMKSMSServiceGetVendPackingSlipJourWithLinesRequest
            {
                packingSlipId = packingSlipId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getVendPackingSlipJourWithLinesAsync(request);

            return new ServiceResponse<VendPackingSlipJourDto>
            {
                Data = _mapper.MapToVendPackingSlipJourDto(response.response),
                Message = "Vend Packing Slip Jour with Lines retrieved successfully",
                Success = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Obsolete("Use GetWorkOrderPagedListV2 instead", true)]
    public async Task<ServiceResponse<PagedListDto<WorkOrderAxDto>>> GetWorkOrderPagedList(int pageNumber, int pageSize, WorkOrderAxSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderPagedListRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderPagedListAsync(request);

            var workOrderPagedList = new PagedListDto<WorkOrderAxDto>(
                _mapper.MapToListOfWorkOrderAxDto(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<WorkOrderAxDto>>
            {
                Data = workOrderPagedList,
                Message = "Work Order Paged List retrieved successfully",
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

            return new ServiceResponse<PagedListDto<WorkOrderAxDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    [Obsolete("Use GetWorkOrderLineListV2 instead", true)]
    public async Task<ServiceResponse<IEnumerable<WorkOrderLineAxDto>>> GetWorkOrderLineList(string workOrderHeaderId)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderLineListRequest
            {
                agsEAMWOID = workOrderHeaderId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderLineListAsync(request);

            return new ServiceResponse<IEnumerable<WorkOrderLineAxDto>>
            {
                Data = _mapper.MapToListOfWorkOrderLineAxDto(response.response),
                Message = "Work Order Line List retrieved successfully",
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

            return new ServiceResponse<IEnumerable<WorkOrderLineAxDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
        finally
        {
            if (_client is GMKSMSServiceClient client)
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    await client.CloseAsync();
                }
            }
        }
    }

    public async Task<ServiceResponse<IEnumerable<InventLocationDto>>> GetInventLocationList(InventLocationDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventLocationListRequest
            {
                parm = _mapper.MapToGMKInventLocationDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventLocationListAsync(request);

            return new ServiceResponse<IEnumerable<InventLocationDto>>
            {
                Data = _mapper.MapToListOfInventLocationDto(response.response),
                Message = "Invent Location List retrieved successfully",
                Success = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}