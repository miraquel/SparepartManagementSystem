using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

internal class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<RoleService>();

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<RoleDto>> AddUser(UserRoleDto dto)
    {
        try
        {
            if (!await _unitOfWork.RoleRepository.AddUser(dto.RoleId, dto.UserId)) throw new InvalidOperationException("Failed to add user to role");

            var role = await _unitOfWork.RoleRepository.GetByIdWithUsers(dto.RoleId);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} added to role {RoleId} successfully", dto.UserId, dto.RoleId);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role),
                Message = "User added successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> DeleteUser(UserRoleDto dto)
    {
        try
        {
            if (!await _unitOfWork.RoleRepository.DeleteUser(dto.RoleId, dto.UserId))
                throw new InvalidOperationException($"Failed to delete user {dto.UserId} from role {dto.RoleId}");

            var role = await _unitOfWork.RoleRepository.GetByIdWithUsers(dto.RoleId);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} deleted from role {RoleId} successfully", dto.UserId, dto.RoleId);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role),
                Message = "User deleted successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllWithUsers()
    {
        try
        {
            var result = (await _unitOfWork.RoleRepository.GetAllWithUsers()).ToList();

            _logger.Information("Roles retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.Map<IEnumerable<RoleDto>>(result),
                Message = "Roles retrieved successfully",
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

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> GetByIdWithUsers(int id)
    {
        try
        {
            var updated = await _unitOfWork.RoleRepository.GetByIdWithUsers(id);

            _logger.Information("Role {RoleId} retrieved successfully", id);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(updated),
                Message = "Role updated successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> Add(RoleDto dto)
    {
        try
        {
            var role = _mapper.Map<Role>(dto);

            var inserted = await _unitOfWork.RoleRepository.Add(role);

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} added successfully", inserted?.RoleId);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(inserted),
                Message = "Role added successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> Delete(int id)
    {
        try
        {
            var deleted = await _unitOfWork.RoleRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} deleted successfully", deleted?.RoleId);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(deleted),
                Message = "Role deleted successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetAll()
    {
        try
        {
            var roles = (await _unitOfWork.RoleRepository.GetAll()).ToList();

            _logger.Information("Roles retrieved successfully with total {TotalRecord} rows", roles.Count);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.Map<IEnumerable<RoleDto>>(roles),
                Message = "Roles retrieved successfully",
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

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> GetById(int id)
    {
        try
        {
            var role = await _unitOfWork.RoleRepository.GetById(id);

            _logger.Information("Role {RoleId} retrieved successfully", id);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role),
                Message = "Role retrieved successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetByParams(RoleDto dto)
    {
        try
        {
            var role = _mapper.Map<Role>(dto);

            var roles = (await _unitOfWork.RoleRepository.GetByParams(role)).ToList();

            _logger.Information("Roles retrieved successfully with total {TotalRecord} rows", roles.Count);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.Map<IEnumerable<RoleDto>>(roles),
                Message = "Roles retrieved successfully",
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

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> Update(RoleDto dto)
    {
        try
        {
            var role = _mapper.Map<Role>(dto);

            var updated = await _unitOfWork.RoleRepository.Update(role);

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} updated successfully", updated?.RoleId);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(updated),
                Message = "Role updated successfully",
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

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}