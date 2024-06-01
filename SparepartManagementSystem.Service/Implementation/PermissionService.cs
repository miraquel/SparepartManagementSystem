using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

internal class PermissionService : IPermissionService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PermissionTypeAccessor _permissionTypeAccessor;
    private readonly ILogger _logger = Log.ForContext<PermissionService>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionService(IUnitOfWork unitOfWork, MapperlyMapper mapper, PermissionTypeAccessor permissionTypeAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _permissionTypeAccessor = permissionTypeAccessor;
        _httpContextAccessor = httpContextAccessor;
    }

    public ServiceResponse<IEnumerable<PermissionDto>> GetAllPermissionTypes()
    {
        try
        {
            var result = _permissionTypeAccessor.AllPermission.ToList();

            _logger.Information("Permission Type retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = result,
                Message = "Permission Type retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public ServiceResponse<IEnumerable<PermissionDto>> GetAllModules()
    {
        try
        {
            var result = _permissionTypeAccessor.AllModule.ToList();

            _logger.Information("Permission Module retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = result,
                Message = "Permission Module retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<PermissionDto>>> GetByRoleId(int roleId)
    {
        try
        {
            var result = (await _unitOfWork.PermissionRepository.GetByRoleId(roleId)).ToList();

            _logger.Information("Permission retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = _mapper.MapToListOfPermissionDto(result),
                Message = "Permission retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> AddPermission(PermissionDto dto)
    {
        try
        {
            var permissionAdd = _mapper.MapToPermission(dto);
            permissionAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            permissionAdd.CreatedDateTime = DateTime.Now;
            permissionAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            permissionAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.PermissionRepository.Add(permissionAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("id: {PermissionId}, Permission added successfully", lastInsertedId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Permission added successfully",
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

    public async Task<ServiceResponse> DeletePermission(int id)
    {
        try
        {
            await _unitOfWork.PermissionRepository.Delete(id);

            _logger.Information("id: {PermissionId}, Permission deleted successfully", id);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Permission deleted successfully",
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

    public async Task<ServiceResponse<IEnumerable<PermissionDto>>> GetAllPermission()
    {
        try
        {
            var result = (await _unitOfWork.PermissionRepository.GetAll()).ToList();

            _logger.Information("Permission retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = _mapper.MapToListOfPermissionDto(result),
                Message = "Permission retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PermissionDto>> GetPermissionById(int id)
    {
        try
        {
            var result = await _unitOfWork.PermissionRepository.GetById(id);

            _logger.Information("id: {PermissionId}, Permission retrieved successfully", result.PermissionId);

            return new ServiceResponse<PermissionDto>
            {
                Data = _mapper.MapToPermissionDto(result),
                Message = "Permission retrieved successfully",
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PermissionDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<PermissionDto>>> GetPermissionByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var result = await _unitOfWork.PermissionRepository.GetByParams(parameters);
            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = _mapper.MapToListOfPermissionDto(result),
                Message = "Permission retrieved successfully",
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

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> UpdatePermission(PermissionDto dto)
    {
        try
        {
            var record = await _unitOfWork.PermissionRepository.GetById(dto.PermissionId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Record has been modified by another user. Please refresh and try again.");
            }

            record.UpdateProperties(_mapper.MapToPermission(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Permission"
                };
            }
            
            record.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            record.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.PermissionRepository.Update(record);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Permission updated successfully",
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
    }
}