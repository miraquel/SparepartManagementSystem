using System.ServiceModel;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class WorkOrderServiceDirect : IWorkOrderServiceDirect
{
    private readonly GMKSMSService _client;
    private readonly CallContext _context;
    private readonly MapperlyMapper _mapper;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();
    private readonly UserClaimDto _userClaim;

    public WorkOrderServiceDirect(GMKSMSService client, CallContext context, MapperlyMapper mapper, UserClaimDto userClaim)
    {
        _client = client;
        _context = context;
        _mapper = mapper;
        _userClaim = userClaim;
    }

    public async Task<ServiceResponse> UpdateWorkOrderHeader(WorkOrderHeaderDto dto)
    {
        try
        {
            var request = new GMKSMSServiceUpdateWorkOrderRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKWorkOrderDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.updateWorkOrderAsync(request);
            
            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse<WorkOrderHeaderDto>> GetWorkOrderHeader(string agsEamWoId)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderRequest
            {
                CallContext = _context,
                agsEAMWOID = agsEamWoId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderAsync(request);

            return new ServiceResponse<WorkOrderHeaderDto>
            {
                Data = _mapper.MapToWorkOrderHeaderDto(response.response),
                Message = "Work Order Header retrieved successfully.",
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

            return new ServiceResponse<WorkOrderHeaderDto>
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

    public async Task<ServiceResponse<PagedListDto<WorkOrderHeaderDto>>> GetWorkOrderHeaderPagedList(int pageNumber, int pageSize, WorkOrderHeaderDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderPagedListRequest
            {
                CallContext = _context,
                pageNumber = pageNumber,
                pageSize = pageSize,
                agsEAMWOID = dto.AGSEAMWOID,
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderPagedListAsync(request);

            return new ServiceResponse<PagedListDto<WorkOrderHeaderDto>>
            {
                Data = new PagedListDto<WorkOrderHeaderDto>(
                    _mapper.MapToListOfWorkOrderHeaderDto(response.response.Items),
                    pageNumber, 
                    pageSize, 
                    response.response.TotalCount),
                Message = "Work Order Header retrieved successfully.",
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

    public async Task<ServiceResponse> AddWorkOrderLine(WorkOrderLineDto dto)
    {
        try
        {
            var request = new GMKSMSServiceAddWorkOrderLineRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKWorkOrderLineDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.addWorkOrderLineAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> UpdateWorkOrderLine(WorkOrderLineDto dto)
    {
        try
        {
            var request = new GMKSMSServiceUpdateWorkOrderLineRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKWorkOrderLineDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.updateWorkOrderLineAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse<WorkOrderLineDto>> GetWorkOrderLine(string agsEamWoId, int line)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderLineRequest
            {
                CallContext = _context,
                agsEAMWOID = agsEamWoId,
                line = line
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getWorkOrderLineAsync(request);

            return new ServiceResponse<WorkOrderLineDto>
            {
                Data = _mapper.MapToWorkOrderLineDto(response.response),
                Message = "Work Order Line retrieved successfully.",
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

            return new ServiceResponse<WorkOrderLineDto>
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

    public async Task<ServiceResponse<IEnumerable<WorkOrderLineDto>>> GetWorkOrderLineList(string agsEamWoId)
    {
        try
        {
            var request = new GMKSMSServiceGetWorkOrderLineListRequest
            {
                CallContext = _context,
                agsEAMWOID = agsEamWoId
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
                Message = "Work Order Line retrieved successfully.",
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

    public async Task<ServiceResponse> CloseWorkOrderLineAndPostInventJournal(WorkOrderLineDto dto)
    {
        try
        {
            var request = new GMKSMSServiceCloseWorkOrderAndPostInventJournalTableRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKWorkOrderLineDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.closeWorkOrderAndPostInventJournalTableAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> AddItemRequisition(InventReqDto dto)
    {
        try
        {
            var request = new GMKSMSServiceAddInventReqRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKInventReqDataContract(dto)
            };

            request.parm.PreparerUserId = _userClaim.Username;

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.addInventReqAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> UpdateItemRequisition(InventReqDto dto)
    {
        try
        {
            var request = new GMKSMSServiceUpdateInventReqRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKInventReqDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.updateInventReqAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }
            
            _logger.Information(response.response.Message);

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> DeleteItemRequisition(InventReqDto dto)
    {
        try
        {
            var request = new GMKSMSServiceDeleteInventReqRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKInventReqDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.deleteInventReqAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> DeleteItemRequisitionWithListOfRecId(IEnumerable<long> recIds)
    {
        try
        {
            var request = new GMKSMSServiceDeleteInventReqWithListOfRecIdRequest
            {
                CallContext = _context,
                recIds = recIds.ToArray()
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.deleteInventReqWithListOfRecIdAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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

    public async Task<ServiceResponse<InventReqDto>> GetItemRequisition(InventReqDto dto)
    {
        try
        {
            var request = new GMKSMSServiceGetInventReqRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKInventReqDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventReqAsync(request);

            return new ServiceResponse<InventReqDto>
            {
                Data = _mapper.MapToInventReqDto(response.response),
                Message = "Item Requisition retrieved successfully.",
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

            return new ServiceResponse<InventReqDto>
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

    public async Task<ServiceResponse<IEnumerable<InventReqDto>>> GetItemRequisitionList(long agsWoRecId)
    {
        try
        {
            var request = new GMKSMSServiceGetInventReqListRequest
            {
                CallContext = _context,
                agsWoRecId = agsWoRecId
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.getInventReqListAsync(request);

            return new ServiceResponse<IEnumerable<InventReqDto>>
            {
                Data = _mapper.MapToListOfInventReqDto(response.response),
                Message = "Item Requisition retrieved successfully.",
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

            return new ServiceResponse<IEnumerable<InventReqDto>>
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

    public async Task<ServiceResponse> CreateInventJournalTable(WorkOrderLineDto dto)
    {
        try
        {
            var request = new GMKSMSServiceCreateInventJournalTableRequest
            {
                CallContext = _context,
                parm = _mapper.MapToGMKWorkOrderLineDataContract(dto)
            };

            if (_client is GMKSMSServiceClient client)
            {
                request.CallContext = _context;
                await client.OpenAsync();
            }

            var response = await _client.createInventJournalTableAsync(request);

            if (!response.response.Success)
            {
                throw new Exception(response.response.Message);
            }

            return new ServiceResponse
            {
                Message = response.response.Message,
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

            return new ServiceResponse
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