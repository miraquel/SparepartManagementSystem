using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class NumberSequenceService : INumberSequenceService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<NumberSequenceService>();

    public NumberSequenceService(IUnitOfWork unitOfWork, MapperlyMapper mapper, RepositoryEvents repositoryEvents)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> AddNumberSequence(NumberSequenceDto dto)
    {
        try
        {
            var numberSequenceAdd = _mapper.MapToNumberSequence(dto);
            await _unitOfWork.NumberSequenceRepository.Add(numberSequenceAdd, _repositoryEvents.OnBeforeAdd);
            
            var lastInsertedId = await _unitOfWork.GetLastInsertedId();

            _logger.Information("id: {NumberSequenceId}, Number Sequence added successfully", lastInsertedId);

            await _unitOfWork.Commit();
            
            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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

    public async Task<ServiceResponse> DeleteNumberSequence(int id)
    {
        try
        {
            await _unitOfWork.NumberSequenceRepository.Delete(id);

            _logger.Information("id: {NumberSequenceId}, Number Sequence deleted successfully", id);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<NumberSequenceDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<NumberSequenceDto>>> GetNumberSequenceByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var result = await _unitOfWork.NumberSequenceRepository.GetByParams(parameters);
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

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
            var record = await _unitOfWork.NumberSequenceRepository.GetById(dto.NumberSequenceId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("Number Sequence has been modified by another user. Please refresh and try again.");
            }

            record.UpdateProperties(_mapper.MapToNumberSequence(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in Number Sequence"
                }; 
            }
            
            await _unitOfWork.NumberSequenceRepository.Update(record, _repositoryEvents.OnBeforeUpdate);

            _logger.Information("id: {NumberSequenceId}, Number Sequence updated successfully", dto.NumberSequenceId);
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "Journal Line added successfully",
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