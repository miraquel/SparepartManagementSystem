using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger = Log.ForContext<RefreshTokenService>();

    public RefreshTokenService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> Add(RefreshTokenDto dto)
    {
        try
        {
            var entity = _mapper.Map<RefreshToken>(dto);
            await _unitOfWork.RefreshTokenRepository.Add(entity);
            var lastInsertedId = _unitOfWork.RefreshTokenRepository.GetLastInsertedId();
            
            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.Add: {lastInsertedId}", lastInsertedId);

            return new ServiceResponse
            {
                Message = "Refresh Token added successfully",
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

    public async Task<ServiceResponse> Delete(int id)
    {
        try
        {
            await _unitOfWork.RefreshTokenRepository.Delete(id);
            
            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.Delete: {@result}", id);

            return new ServiceResponse
            {
                Message = "Refresh Token deleted successfully",
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

    public async Task<ServiceResponse<IEnumerable<RefreshTokenDto>>> GetAll()
    {
        try
        {
            var result = await _unitOfWork.RefreshTokenRepository.GetAll();

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.GetAll: {@result}", result);

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Data = _mapper.Map<IEnumerable<RefreshTokenDto>>(result),
                Message = "Refresh Token retrieved successfully",
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

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RefreshTokenDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.RefreshTokenRepository.GetById(id);

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.GetById: {@result}", result);

            return new ServiceResponse<RefreshTokenDto>
            {
                Data = _mapper.Map<RefreshTokenDto>(result),
                Message = "Refresh Token retrieved successfully",
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

            return new ServiceResponse<RefreshTokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RefreshTokenDto>>> GetByParams(RefreshTokenDto dto)
    {
        try
        {
            var entity = _mapper.Map<RefreshToken>(dto);
            var result = await _unitOfWork.RefreshTokenRepository.GetByParams(entity);

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.GetByParams: {@result}", result);

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Data = _mapper.Map<IEnumerable<RefreshTokenDto>>(result),
                Message = "Refresh Token retrieved successfully",
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

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> Update(RefreshTokenDto dto)
    {
        try
        {
            var entity = _mapper.Map<RefreshToken>(dto);
            await _unitOfWork.RefreshTokenRepository.Update(entity);

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.Update: {@result}", entity);

            return new ServiceResponse
            {
                Message = "Refresh Token updated successfully",
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
            var result = await _unitOfWork.RefreshTokenRepository.GetLastInsertedId();

            _logger.Information("Refresh token last inserted id retrieved successfully, id: {LastInsertedId}", result);

            return new ServiceResponse<int>
            {
                Data = result,
                Message = "Refresh token last inserted id retrieved successfully",
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

    public async Task<ServiceResponse<RefreshTokenDto>> Revoke(RefreshTokenDto dto)
    {
        try
        {
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(dto.UserId, dto.Token) ?? throw new InvalidOperationException("Refresh Token not found");

            var result = await _unitOfWork.RefreshTokenRepository.Revoke(refreshToken.RefreshTokenId);

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.Revoke: {@result}", result);

            return new ServiceResponse<RefreshTokenDto>
            {
                Data = _mapper.Map<RefreshTokenDto>(result),
                Message = "Refresh Token revoked successfully",
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

            return new ServiceResponse<RefreshTokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RefreshTokenDto>> RevokeAll(int userId)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetById(userId) ?? throw new InvalidOperationException("Refresh Token not found");

            var result = await _unitOfWork.RefreshTokenRepository.RevokeAll(user.UserId);

            _unitOfWork.Commit();

            _logger.Information("RefreshTokenService.RevokeAll: {@result}", result);

            return new ServiceResponse<RefreshTokenDto>
            {
                Data = _mapper.Map<RefreshTokenDto>(result),
                Message = "Refresh Token revoked successfully",
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

            return new ServiceResponse<RefreshTokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}