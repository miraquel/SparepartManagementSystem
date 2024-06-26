using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

internal class RoleService : IRoleService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<RoleService>();

    public RoleService(IUnitOfWork unitOfWork, MapperlyMapper mapper, RepositoryEvents repositoryEvents)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> AddUserToRole(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.AddUser(dto.RoleId, dto.UserId);

            _logger.Information("User {UserId} added to role {RoleId} successfully", dto.UserId, dto.RoleId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "User added successfully",
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

    public async Task<ServiceResponse> DeleteUserFromRole(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.DeleteUser(dto.RoleId, dto.UserId);

            _logger.Information("User {UserId} deleted from role {RoleId} successfully", dto.UserId, dto.RoleId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "User deleted successfully",
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

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllRoleWithUsers()
    {
        try
        {
            var result = (await _unitOfWork.RoleRepository.GetAllWithUsers()).ToList();

            _logger.Information("Roles retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.MapToListOfRoleDto(result),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> GetRoleByIdWithUsers(int id)
    {
        try
        {
            var updated = await _unitOfWork.RoleRepository.GetByIdWithUsers(id);

            _logger.Information("Role {RoleId} retrieved successfully", id);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.MapToRoleDto(updated),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> AddRole(RoleDto dto)
    {
        try
        {
            var roleAdd = _mapper.MapToRole(dto);
            await _unitOfWork.RoleRepository.Add(roleAdd, _repositoryEvents.OnBeforeAdd);
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("Role {RoleId} added successfully", lastInsertedId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Role added successfully",
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

    public async Task<ServiceResponse> DeleteRole(int id)
    {
        try
        {
            await _unitOfWork.RoleRepository.Delete(id);

            _logger.Information("Role {RoleId} deleted successfully", id);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Role deleted successfully",
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

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllRole()
    {
        try
        {
            var roles = (await _unitOfWork.RoleRepository.GetAll()).ToList();

            _logger.Information("Roles retrieved successfully with total {TotalRecord} rows", roles.Count);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.MapToListOfRoleDto(roles),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RoleDto>> GetRoleById(int id)
    {
        try
        {
            var role = await _unitOfWork.RoleRepository.GetById(id);

            _logger.Information("Role {RoleId} retrieved successfully", id);

            return new ServiceResponse<RoleDto>
            {
                Data = _mapper.MapToRoleDto(role),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<RoleDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RoleDto>>> GetRoleByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var roles = await _unitOfWork.RoleRepository.GetByParams(parameters);
            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Data = _mapper.MapToListOfRoleDto(roles),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<RoleDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> UpdateRole(RoleDto dto)
    {
        try
        {
            var record = await _unitOfWork.RoleRepository.GetById(dto.RoleId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Role has been modified by another user. Please refresh the page and try again.");
            }

            record.UpdateProperties(_mapper.MapToRole(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Role"
                };
            }

            await _unitOfWork.RoleRepository.Update(record, _repositoryEvents.OnBeforeUpdate);

            _logger.Information("Role {RoleId} updated successfully", dto.RoleId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Role updated successfully",
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
}