using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class UserWarehouseService : IUserWarehouseService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptService>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserWarehouseService(MapperlyMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<UserWarehouseDto>> GetUserWarehouseById(int userWarehouseId)
    {
        try
        {
            var userWarehouse = await _unitOfWork.UserWarehouseRepository.GetById(userWarehouseId);

            return new ServiceResponse<UserWarehouseDto>
            {
                Success = true,
                Data = _mapper.MapToUserWarehouseDto(userWarehouse)
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

            return new ServiceResponse<UserWarehouseDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetAllUserWarehouse()
    {
        try
        {
            var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetAll();

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Success = true,
                Data = _mapper.MapToListOfUserWarehouseDto(userWarehouses)
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

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetUserWarehouseByParams(UserWarehouseDto userWarehouseDto)
    {
        try
        {
            var userWarehouse = _mapper.MapToUserWarehouse(userWarehouseDto);
            var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetByParams(userWarehouse);

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Success = true,
                Data = _mapper.MapToListOfUserWarehouseDto(userWarehouses)
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

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserWarehouseDto>>> GetUserWarehouseByUserId(int userId)
    {
        try
        {
            var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetByUserId(userId);

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Success = true,
                Data = _mapper.MapToListOfUserWarehouseDto(userWarehouses),
                Message = "User Warehouses fetched successfully"
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

            return new ServiceResponse<IEnumerable<UserWarehouseDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> AddUserWarehouse(UserWarehouseDto userWarehouseDto)
    {
        try
        {
            // prevent adding a default warehouse if the user already has one
            if (userWarehouseDto.IsDefault)
            {
                var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetByUserId(userWarehouseDto.UserId);
                if (userWarehouses.Any(uw => uw.IsDefault is true))
                {
                    throw new InvalidOperationException("Cannot add default warehouse, user already has one");
                }
            }
            
            var userWarehouseAdd = _mapper.MapToUserWarehouse(userWarehouseDto);
            userWarehouseAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            userWarehouseAdd.CreatedDateTime = DateTime.Now;
            userWarehouseAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            userWarehouseAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.UserWarehouseRepository.Add(userWarehouseAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            _logger.Information("UserWarehouse added successfully, UserWarehouseId: {UserWarehouseId}", lastInsertedId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "User Warehouse added successfully"
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

    public async Task<ServiceResponse> UpdateUserWarehouse(UserWarehouseDto userWarehouseDto)
    {
        try
        {
            // prevent update if the user has a default warehouse
            var userWarehouses = await _unitOfWork.UserWarehouseRepository.GetByUserId(userWarehouseDto.UserWarehouseId, true);
            var oldRecords = userWarehouses as UserWarehouse[] ?? userWarehouses.ToArray();
            if (oldRecords.Any(uw => uw.IsDefault is true))
            {
                throw new InvalidOperationException("Cannot update warehouse, user already has a default warehouse");
            }
            
            var oldRecord = oldRecords.FirstOrDefault(uw => uw.UserWarehouseId == userWarehouseDto.UserWarehouseId) ?? throw new InvalidOperationException("User Warehouse not found");
            if (oldRecord.ModifiedDateTime > userWarehouseDto.ModifiedDateTime)
            {
                throw new InvalidOperationException("User Warehouse has been modified by another user, please refresh and try again");
            }
            
            var newRecord = _mapper.MapToUserWarehouse(userWarehouseDto);
            newRecord.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            newRecord.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.UserWarehouseRepository.Update(UserWarehouse.ForUpdate(oldRecord, newRecord));
            
            _logger.Information("UserWarehouse updated successfully, UserWarehouseId: {UserWarehouseId}", newRecord.UserWarehouseId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "User Warehouse updated successfully"
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

    public async Task<ServiceResponse> DeleteUserWarehouse(int userWarehouseId)
    {
        try
        {
            var userWarehouse = await _unitOfWork.UserWarehouseRepository.GetById(userWarehouseId);
            // if user warehouse is default, throw an error
            if (userWarehouse.IsDefault is true)
            {
                throw new InvalidOperationException("Cannot delete default warehouse");
            }
            await _unitOfWork.UserWarehouseRepository.Delete(userWarehouseId);
            
            _logger.Information("UserWarehouse deleted successfully, UserWarehouseId: {UserWarehouseId}", userWarehouseId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "User Warehouse deleted successfully"
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

    public async Task<ServiceResponse<UserWarehouseDto>> GetDefaultUserWarehouseByUserId(int userId)
    {
        try
        {
            var userWarehouse = await _unitOfWork.UserWarehouseRepository.GetDefaultByUserId(userId);

            return new ServiceResponse<UserWarehouseDto>
            {
                Success = true,
                Message = "Default User Warehouse fetched successfully",
                Data = _mapper.MapToUserWarehouseDto(userWarehouse)
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

            return new ServiceResponse<UserWarehouseDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}