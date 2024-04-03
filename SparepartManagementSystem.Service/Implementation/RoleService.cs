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

    public async Task<ServiceResponse> AddUser(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.AddUser(dto.RoleId, dto.UserId);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} added to role {RoleId} successfully", dto.UserId, dto.RoleId);

            return new ServiceResponse
            {
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

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> DeleteUser(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.DeleteUser(dto.RoleId, dto.UserId);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} deleted from role {RoleId} successfully", dto.UserId, dto.RoleId);

            return new ServiceResponse
            {
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> Add(RoleDto dto)
    {
        try
        {
            var role = _mapper.Map<Role>(dto);

            await _unitOfWork.RoleRepository.Add(role);
            var lastInsertedId = await _unitOfWork.RoleRepository.GetLastInsertedId();

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} added successfully", lastInsertedId);

            return new ServiceResponse
            {
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
            await _unitOfWork.RoleRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} deleted successfully", id);

            return new ServiceResponse
            {
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

            return new ServiceResponse
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

    public async Task<ServiceResponse> Update(RoleDto dto)
    {
        try
        {
            var role = _mapper.Map<Role>(dto);

            await _unitOfWork.RoleRepository.Update(role);

            _unitOfWork.Commit();

            _logger.Information("Role {RoleId} updated successfully", role.RoleId);

            return new ServiceResponse
            {
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
            var lastInsertedId = await _unitOfWork.RoleRepository.GetLastInsertedId();

            _logger.Information("Role last inserted id retrieved successfully, id: {LastInsertedId}", lastInsertedId);

            return new ServiceResponse<int>
            {
                Data = lastInsertedId,
                Message = "Role last inserted id retrieved successfully",
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