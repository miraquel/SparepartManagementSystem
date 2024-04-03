using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class RowLevelAccessService : IRowLevelAccessService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<RoleService>();

    public RowLevelAccessService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> Add(RowLevelAccessDto dto)
    {
        try
        {
            await _unitOfWork.RowLevelAccessRepository.Add(_mapper.Map<RowLevelAccess>(dto));
            
            _unitOfWork.Commit();
            
            _logger.Information("Row Level Access with id {rowLevelAccessId} added successfully", dto.RowLevelAccessId);
            
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
    public async Task<ServiceResponse> Delete(int id)
    {
        try
        {
            await _unitOfWork.RowLevelAccessRepository.Delete(id);
            
            _unitOfWork.Commit();
            
            _logger.Information("Row Level Access with id {rowLevelAccessId} deleted successfully", id);
            
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
    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetAll()
    {
        try
        {
            var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetAll();
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.Map<IEnumerable<RowLevelAccessDto>>(rowLevelAccesses),
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
    public async Task<ServiceResponse<RowLevelAccessDto>> GetById(int id)
    {
        try
        {
            var rowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetById(id);
            
            return new ServiceResponse<RowLevelAccessDto>
            {
                Data = _mapper.Map<RowLevelAccessDto>(rowLevelAccess),
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
    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetByParams(RowLevelAccessDto dto)
    {
        try
        {
            var rowLevelAccess = await _unitOfWork.RowLevelAccessRepository.GetByParams(_mapper.Map<RowLevelAccess>(dto));
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.Map<IEnumerable<RowLevelAccessDto>>(rowLevelAccess),
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
    public async Task<ServiceResponse> Update(RowLevelAccessDto dto)
    {
        try
        {
            await _unitOfWork.RowLevelAccessRepository.Update(_mapper.Map<RowLevelAccess>(dto));

            _unitOfWork.Commit();

            _logger.Information("Row Level Access with id {rowLevelAccessId} updated successfully", dto.RowLevelAccessId);

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
    public async Task<ServiceResponse<int>> GetLastInsertedId()
    {
        try
        {
            var lastInsertedId = await _unitOfWork.RowLevelAccessRepository.GetLastInsertedId();
            
            _logger.Information("Row Level Access last inserted id retrieved successfully, id: {LastInsertedId}", lastInsertedId);
            
            return new ServiceResponse<int>
            {
                Data = lastInsertedId,
                Message = "Row level access last inserted id retrieved successfully",
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

    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> GetByUserId(int userId)
    {
        try
        {
            var rowLevelAccesses = await _unitOfWork.RowLevelAccessRepository.GetByUserId(userId);
            
            _logger.Information("Row Level Accesses with UserId {userId} retrieved successfully", userId);
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Data = _mapper.Map<IEnumerable<RowLevelAccessDto>>(rowLevelAccesses),
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
    public async Task<ServiceResponse<IEnumerable<RowLevelAccessDto>>> BulkDelete(IEnumerable<int> ids)
    {
        try
        {
            var idsArray = ids as int[] ?? ids.ToArray();
            foreach (var id in idsArray)
            {
                await _unitOfWork.RowLevelAccessRepository.Delete(id);
            }
            
            _unitOfWork.Commit();
            
            _logger.Information("Row Level Accesses with ids {ids} deleted successfully", string.Join(", ", idsArray));
            
            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
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

            return new ServiceResponse<IEnumerable<RowLevelAccessDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}