using System.Net;
using System.ServiceModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Shared.Helper;

namespace SparepartManagementSystem.Service.Implementation;

public class GMKSMSServiceGroupImplementation : IGMKSMSServiceGroup
{
    private readonly GMKSMSService _client;
    private readonly CallContext _context;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GMKSMSServiceGroupImplementation(GMKSMSService client, CallContext context, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _client = client;
        _context = context;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<PagedListDto<InventTableDto>>> GetInventTablePagedList(int pageNumber, int pageSize, InventTableSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventTablePagedListRequest()
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                productName = dto.ProductName,
                searchName = dto.SearchName,
                itemId = dto.ItemId,
            };
            
            var isUserIdExists = int.TryParse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value, out var userId);
            if (isUserIdExists)
            {
                var rowLevelAccesses = (await _unitOfWork.RowLevelAccessRepository.GetByUserId(userId)).ToArray();
                var filters = rowLevelAccesses.Where(x => x.AxTable == AxTable.InventTable).Select(x => x.Query).ToArray();
                request.itemIdFilters = filters.FilterAndFormatStringsByPrefix(dto.ItemId).ToArray();
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventTablePagedListAsync(request);

            var inventTablePagedList = new PagedListDto<InventTableDto>(
                _mapper.Map<IEnumerable<InventTableDto>>(response.response.Items.Select(x =>
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServiceGetPurchTablePagedListRequest()
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                purchId = dto.PurchId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getPurchTablePagedListAsync(request);

            var purchTablePagedList = new PagedListDto<PurchTableDto>(
                _mapper.Map<List<PurchTableDto>>(response.response.Items),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServiceGetPurchLineListRequest()
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
                Data = _mapper.Map<IEnumerable<PurchLineDto>>(response.response),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServiceGetWMSLocationPagedListRequest()
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                inventLocationId = dto.InventLocationId,
                wMSLocationId = dto.WMSLocationId,
            };
            
            var isUserIdExists = int.TryParse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value, out var userId);
            if (isUserIdExists)
            {
                var rowLevelAccesses = (await _unitOfWork.RowLevelAccessRepository.GetByUserId(userId)).ToArray();
                var filters = rowLevelAccesses.Where(x => x.AxTable == AxTable.InventLocation).Select(x => x.Query).ToArray();
                request.filters = filters;
            }

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWMSLocationPagedListAsync(request);

            var wmsLocationPagedList = new PagedListDto<WMSLocationDto>(
                _mapper.Map<IEnumerable<WMSLocationDto>>(response.response.Items),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServicePostPurchPackingSlipRequest()
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
                Data = _mapper.Map<GMKServiceResponseDto>(response.response),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServiceGetPurchTableRequest()
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
                Data = _mapper.Map<PurchTableDto>(response.response),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
            var request = new GMKSMSServiceGetInventSumListRequest()
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
                Data = _mapper.Map<IEnumerable<InventSumDto>>(response.response),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
    public async Task<ServiceResponse<PagedListDto<WorkOrderDto>>> GetWorkOrderPagedList(int pageNumber, int pageSize, WorkOrderSearchDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderPagedListRequest()
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                agsEAMWOID = dto.AGSEAMWOID
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderPagedListAsync(request);

            var workOrderPagedList = new PagedListDto<WorkOrderDto>(
                _mapper.Map<IEnumerable<WorkOrderDto>>(response.response.Items),
                response.response.PageNumber,
                response.response.PageSize,
                response.response.TotalCount);

            return new ServiceResponse<PagedListDto<WorkOrderDto>>
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            return new ServiceResponse<PagedListDto<WorkOrderDto>>
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
}