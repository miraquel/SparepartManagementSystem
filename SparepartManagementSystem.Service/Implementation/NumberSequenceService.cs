using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class NumberSequenceService : INumberSequenceService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<NumberSequenceService>();

    public NumberSequenceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> Add(NumberSequenceDto dto)
    {
        try
        {
            await _unitOfWork.NumberSequenceRepository.Add(_mapper.Map<NumberSequence>(dto));
            var lastInsertedId = await _unitOfWork.NumberSequenceRepository.GetLastInsertedId();

            _unitOfWork.Commit();

            _logger.Information("id: {NumberSequenceId}, Number Sequence added successfully", lastInsertedId);

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

    public async Task<ServiceResponse> Delete(int id)
    {
        try
        {
            await _unitOfWork.NumberSequenceRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {NumberSequenceId}, Number Sequence deleted successfully", id);

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

    public async Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetAll()
    {
        try
        {
            var result = (await _unitOfWork.NumberSequenceRepository.GetAll()).ToList();

            _logger.Information("Number Sequence get all successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Data = _mapper.Map<IEnumerable<NumberSequenceDto>>(result),
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

    public async Task<ServiceResponse<NumberSequenceDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.NumberSequenceRepository.GetById(id);

            _logger.Information("id: {NumberSequenceId}, Number Sequence retrieved successfully", result?.NumberSequenceId);

            return new ServiceResponse<NumberSequenceDto>
            {
                Data = _mapper.Map<NumberSequenceDto>(result),
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

    public async Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetByParams(NumberSequenceDto dto)
    {
        try
        {
            var result = (await _unitOfWork.NumberSequenceRepository.GetByParams(_mapper.Map<NumberSequence>(dto))).ToList();

            _logger.Information("Number Sequence get by params successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<NumberSequenceDto>>
            {
                Data = _mapper.Map<IEnumerable<NumberSequenceDto>>(result),
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

    public async Task<ServiceResponse> Update(NumberSequenceDto dto)
    {
        try
        {
            await _unitOfWork.NumberSequenceRepository.Update(_mapper.Map<NumberSequence>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {NumberSequenceId}, Number Sequence updated successfully", dto.NumberSequenceId);

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
    public async Task<ServiceResponse<int>> GetLastInsertedId()
    {
        try
        {
            var result = await _unitOfWork.NumberSequenceRepository.GetLastInsertedId();

            _logger.Information("Number Sequence last inserted id retrieved successfully, id: {LastInsertedId}", result);

            return new ServiceResponse<int>
            {
                Data = result,
                Message = "Number sequence last inserted id retrieved successfully",
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