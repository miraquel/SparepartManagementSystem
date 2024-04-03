using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

internal class PermissionService : IPermissionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PermissionTypeAccessor _permissionTypeAccessor;
    private readonly ILogger _logger = Log.ForContext<PermissionService>();

    public PermissionService(IUnitOfWork unitOfWork, IMapper mapper, PermissionTypeAccessor permissionTypeAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _permissionTypeAccessor = permissionTypeAccessor;
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

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
                Data = _mapper.Map<IEnumerable<PermissionDto>>(result),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> Add(PermissionDto dto)
    {
        try
        {
            var permission = _mapper.Map<Permission>(dto);

            await _unitOfWork.PermissionRepository.Add(permission);
            var lastInsertedId = await _unitOfWork.NumberSequenceRepository.GetLastInsertedId();

            _unitOfWork.Commit();

            _logger.Information("id: {PermissionId}, Permission added successfully", lastInsertedId); 

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
            await _unitOfWork.PermissionRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {PermissionId}, Permission deleted successfully", id);

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

    public async Task<ServiceResponse<IEnumerable<PermissionDto>>> GetAll()
    {
        try
        {
            var result = (await _unitOfWork.PermissionRepository.GetAll()).ToList();

            _logger.Information("Permission retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = _mapper.Map<IEnumerable<PermissionDto>>(result),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PermissionDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.PermissionRepository.GetById(id);

            _logger.Information("id: {PermissionId}, Permission retrieved successfully", result?.PermissionId);

            return new ServiceResponse<PermissionDto>
            {
                Data = _mapper.Map<PermissionDto>(result),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PermissionDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<PermissionDto>>> GetByParams(PermissionDto dto)
    {
        try
        {
            var permission = _mapper.Map<Permission>(dto);

            var result = (await _unitOfWork.PermissionRepository.GetByParams(permission)).ToList();

            _logger.Information("Permission retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Data = _mapper.Map<IEnumerable<PermissionDto>>(result),
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

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<PermissionDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> Update(PermissionDto dto)
    {
        try
        {
            var permission = _mapper.Map<Permission>(dto);

            await _unitOfWork.PermissionRepository.Update(permission);

            _unitOfWork.Commit();

            _logger.Information("id: {PermissionId}, Permission updated successfully", dto.PermissionId);

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
            var result = await _unitOfWork.PermissionRepository.GetLastInsertedId();

            _logger.Information("Permission last inserted id retrieved successfully, id: {LastInsertedId}", result);

            return new ServiceResponse<int>
            {
                Data = result,
                Message = "Permission last inserted id retrieved successfully",
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
}