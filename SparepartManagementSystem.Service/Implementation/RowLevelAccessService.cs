using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class RowLevelAccessService : IRowLevelAccessService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<RoleService>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RowLevelAccessService(IUnitOfWork unitOfWork, MapperlyMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse> AddRowLevelAccess(RowLevelAccessDto dto)
    {
        try
        {
            var roleLevelAccessAdd = _mapper.MapToRowLevelAccess(dto);
            roleLevelAccessAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            roleLevelAccessAdd.CreatedDateTime = DateTime.Now;
            roleLevelAccessAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            roleLevelAccessAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.RowLevelAccessRepository.Add(roleLevelAccessAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();
            
            _logger.Information("Row Level Access with id {rowLevelAccessId} added successfully", lastInsertedId);
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Message = "Row Level Access added successfully",
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
    public async Task<ServiceResponse> DeleteRowLevelAccess(int id)
    {
        try
        {
            await _unitOfWork.RowLevelAccessRepository.Delete(id);
            
            _logger.Information("Row Level Access with id {rowLevelAccessId} deleted successfully", id);
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Message = "Row Level Access deleted successfully",
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
    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetAllRowLevelAccess()
    {
        try
        {
            var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetAll();
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.MapToListOfRowLevelAccessDto(rowLevelAccesses),
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

            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<RowLevelAccessDto>> GetRowLevelAccessById(int id)
    {
        try
        {
            var rowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetById(id);
            
            return new ServiceResponse<RowLevelAccessDto>
            {
                Data = _mapper.MapToRowLevelAccessDto(rowLevelAccess),
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

            return new ServiceResponse<RowLevelAccessDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetRowLevelAccessByParams(RowLevelAccessDto dto)
    {
        try
        {
            var rowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetByParams(_mapper.MapToRowLevelAccess(dto));
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.MapToListOfRowLevelAccessDto(rowLevelAccess),
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

            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse> UpdateRowLevelAccess(RowLevelAccessDto dto)
    {
        try
        {
            var oldRecord = await _unitOfWork.RowLevelAccessRepository.GetById(dto.RowLevelAccessId, true);

            if (oldRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Row Level Access has been modified by another user. Please refresh the page and try again.");
            }
            
            var newRecord = _mapper.MapToRowLevelAccess(dto);
            newRecord.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            newRecord.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.RowLevelAccessRepository.Update(RowLevelAccess.ForUpdate(oldRecord, newRecord));

            _logger.Information("Row Level Access with id {rowLevelAccessId} updated successfully", dto.RowLevelAccessId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Row Level Access updated successfully",
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

    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetRowLevelAccessByUserId(int userId)
    {
        try
        {
            var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(userId);
            
            _logger.Information("Row Level Accesses with UserId {userId} retrieved successfully", userId);
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.MapToListOfRowLevelAccessDto(rowLevelAccesses),
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

            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse> BulkDeleteRowLevelAccess(IEnumerable<int> ids)
    {
        try
        {
            var idsArray = ids as int[] ?? ids.ToArray();
            foreach (var id in idsArray)
            {
                await _unitOfWork.RowLevelAccessRepository.Delete(id);
            }
            
            _logger.Information("Row Level Accesses with ids {ids} deleted successfully", string.Join(", ", idsArray));
            
            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Message = "Row Level Accesses deleted successfully",
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
}