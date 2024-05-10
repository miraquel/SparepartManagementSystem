using Microsoft.AspNetCore.Http;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class NumberSequenceService : INumberSequenceService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<NumberSequenceService>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NumberSequenceService(IUnitOfWork unitOfWork, MapperlyMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse> AddNumberSequence(NumberSequenceDto dto)
    {
        try
        {
            var numberSequenceAdd = _mapper.MapToNumberSequence(dto);
            numberSequenceAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            numberSequenceAdd.CreatedDateTime = DateTime.Now;
            numberSequenceAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            numberSequenceAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.NumberSequenceRepository.Add(numberSequenceAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("id: {NumberSequenceId}, Number Sequence added successfully", lastInsertedId);

            _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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

    public async Task<ServiceResponse> DeleteNumberSequence(int id)
    {
        try
        {
            await _unitOfWork.NumberSequenceRepository.Delete(id);

            _logger.Information("id: {NumberSequenceId}, Number Sequence deleted successfully", id);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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

    public async Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetAllNumberSequence()
    {
        try
        {
            var result = (await _unitOfWork.NumberSequenceRepository.GetAll()).ToList();

            _logger.Information("Number Sequence get all successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Data = _mapper.MapToListOfNumberSequenceDto(result),
                Message = "Journal Line added successfully",
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

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<NumberSequenceDto>> GetNumberSequenceById(int id)
    {
        try
        {
            var result = await _unitOfWork.NumberSequenceRepository.GetById(id);

            _logger.Information("id: {NumberSequenceId}, Number Sequence retrieved successfully", result.NumberSequenceId);

            return new ServiceResponse<NumberSequenceDto>
            {
                Data = _mapper.MapToNumberSequenceDto(result),
                Message = "Journal Line added successfully",
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

            return new ServiceResponse<NumberSequenceDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetNumberSequenceByParams(NumberSequenceDto dto)
    {
        try
        {
            var result = (await _unitOfWork.NumberSequenceRepository.GetByParams(_mapper.MapToNumberSequence(dto))).ToList();

            _logger.Information("Number Sequence get by params successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Data = _mapper.MapToListOfNumberSequenceDto(result),
                Message = "Journal Line added successfully",
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

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> UpdateNumberSequence(NumberSequenceDto dto)
    {
        try
        {
            var oldRecord = await _unitOfWork.NumberSequenceRepository.GetById(dto.NumberSequenceId, true);

            if (oldRecord.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Number Sequence has been modified by another user. Please refresh and try again.");
            }
            
            var newRecord = _mapper.MapToNumberSequence(dto);
            newRecord.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            newRecord.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.NumberSequenceRepository.Update(NumberSequence.ForUpdate(oldRecord, newRecord));

            _logger.Information("id: {NumberSequenceId}, Number Sequence updated successfully", dto.NumberSequenceId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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