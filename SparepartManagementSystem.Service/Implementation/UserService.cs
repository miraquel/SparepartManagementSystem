using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

internal class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger = Log.ForContext<UserService>();
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoginService _loginService;
    private readonly IActiveDirectoryService _activeDirectoryService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, ILoginService loginService, IActiveDirectoryService activeDirectoryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _loginService = loginService;
        _activeDirectoryService = activeDirectoryService;
    }

    public async Task<ServiceResponse<UserDto>> Add(UserDto dto)
    {
        try
        {
            var user = _mapper.Map<User>(dto);

            var inserted = await _unitOfWork.UserRepository.Add(user);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} added successfully", inserted?.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(inserted),
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> Delete(int id)
    {
        try
        {
            var deleted = await _unitOfWork.UserRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} deleted successfully", deleted?.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(deleted),
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserDto>>> GetAll()
    {
        try
        {
            var users = (await _unitOfWork.UserRepository.GetAll()).ToList();

            _logger.Information("Users retrieved successfully with total {TotalRecord} rows", users.Count);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(users),
                Message = "Users retrieved successfully",
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

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetById(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            _logger.Information("User {UserId} retrieved successfully", user?.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(user),
                Message = "User retrieved successfully",
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserDto>>> GetByParams(UserDto dto)
    {
        try
        {
            var user = _mapper.Map<User>(dto);

            var users = (await _unitOfWork.UserRepository.GetByParams(user)).ToList();

            _logger.Information("Users retrieved successfully with total {TotalRecord} rows", users.Count);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(users),
                Message = "Users retrieved successfully",
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

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> Update(UserDto dto)
    {
        try
        {
            var user = _mapper.Map<User>(dto);

            var updated = await _unitOfWork.UserRepository.Update(user);

            _unitOfWork.Commit();

            _logger.Information("User {UserId} updated successfully", updated?.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(updated),
                Message = "User updated successfully",
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> AddRole(UserRoleDto dto)
    {
        try
        {
            if (!await _unitOfWork.RoleRepository.AddUser(dto.RoleId, dto.UserId))
                throw new InvalidOperationException("Failed to add role to user");

            var user = await _unitOfWork.UserRepository.GetByIdWithRoles(dto.UserId);

            _unitOfWork.Commit();

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(user),
                Message = "Role added to user successfully",
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> DeleteRole(UserRoleDto dto)
    {
        try
        {
            if (!await _unitOfWork.RoleRepository.DeleteUser(dto.RoleId, dto.UserId))
                throw new InvalidOperationException($"Failed to delete role {dto.UserId} from user {dto.RoleId}");

            var user = await _unitOfWork.UserRepository.GetByIdWithRoles(dto.UserId);

            _unitOfWork.Commit();

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(user),
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserDto>>> GetAllWithRoles()
    {
        try
        {
            var result = (await _unitOfWork.UserRepository.GetAllWithRoles()).ToList();

            _logger.Information("Users retrieved successfully with total {TotalRecord} rows", result.Count);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(result),
                Message = "User updated successfully",
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

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetByIdWithRoles(int id)
    {
        try
        {
            var result = await _unitOfWork.UserRepository.GetByIdWithRoles(id);

            _logger.Information("User {UserId} retrieved successfully", result?.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(result),
                Message = "User updated successfully",
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetByUsernameWithRoles(string username)
    {
        try
        {
            var result = await _unitOfWork.UserRepository.GetByUsernameWithRoles(username);

            _logger.Information("User {Username} retrieved successfully", result?.Username);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.Map<UserDto>(result),
                Message = "User retrieved successfully",
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

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public ServiceResponse<IEnumerable<UserDto>> GetUsersFromActiveDirectory()
    {
        try
        {
            var users = _activeDirectoryService.GetUsersFromActiveDirectory();

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(users),
                Message = "Active Directory Users retrieved",
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

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<TokenDto>> LoginWithActiveDirectory(UsernamePasswordDto usernamePassword)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByUsername(usernamePassword.Username) ?? throw new InvalidOperationException(
                $"User {usernamePassword.Username} not found");

            if (!_loginService.LoginWithUsernameAndPassword(usernamePassword.Username, usernamePassword.Password))
                throw new InvalidOperationException("Login failed");

            var refreshTokenResult = _loginService.GenerateToken(user, true);

            await _unitOfWork.RefreshTokenRepository.Add(new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshTokenResult,
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(
                    _configuration.GetSection("Jwt:RefreshTokenExpiration").Value ??
                    throw new InvalidOperationException("Jwt Configuration not set properly, missing Refresh Token Expiration"))),
                Created = DateTime.UtcNow
            });

            var token = new TokenDto
            {
                AccessToken = _loginService.GenerateToken(user),
                RefreshToken = refreshTokenResult
            };

            _unitOfWork.Commit();

            _logger.Information("User {Username} logged in successfully, {User}", user.Username, JsonConvert.SerializeObject(user));

            return new ServiceResponse<TokenDto>
            {
                Data = token,
                Message = "User logged in successfully",
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

            return new ServiceResponse<TokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<TokenDto>> RefreshToken(string token)
    {
        try
        {
            var refreshTokenResult = "";

            var principal = _loginService.ValidateToken(token);

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ??
                         throw new InvalidOperationException("Invalid token");
            var user = await _unitOfWork.UserRepository.GetByIdWithRoles(int.Parse(userId)) ??
                       throw new InvalidOperationException($"UserId {userId} not found");
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(user.UserId, token) ??
                               throw new InvalidOperationException($"RefreshToken {token} with UserId {userId} not found");

            if (!refreshToken.IsActive) throw new InvalidOperationException("Invalid token");

            if (refreshToken is { IsActive: true, IsExpired: true })
            {
                var newRefreshToken = _loginService.GenerateToken(user, true);
                refreshTokenResult = newRefreshToken;

                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.ReplacedByToken = newRefreshToken;
                await _unitOfWork.RefreshTokenRepository.Update(refreshToken);

                var newRefreshTokenEntity = new RefreshToken
                {
                    UserId = user.UserId,
                    Token = newRefreshToken,
                    Expires = DateTime.UtcNow.AddSeconds(int.Parse(
                        _configuration.GetSection("Jwt:RefreshTokenExpiration").Value ??
                        throw new InvalidOperationException("Jwt Configuration not set properly, missing Refresh Token Expiration"))),
                    Created = DateTime.UtcNow
                };
                await _unitOfWork.RefreshTokenRepository.Add(newRefreshTokenEntity);
            }

            var accessTokenResult = _loginService.GenerateToken(user);

            var result = new TokenDto
            {
                AccessToken = accessTokenResult,
                RefreshToken = refreshTokenResult
            };

            _logger.Information("Token refreshed successfully, {Token}", JsonConvert.SerializeObject(result));

            return new ServiceResponse<TokenDto>
            {
                Data = result,
                Message = "Token refreshed successfully",
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

            if (ex.StackTrace != null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<TokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<RefreshTokenDto>>> RevokeAllTokens(int userId)
    {
        try
        {
            _ = _unitOfWork.RefreshTokenRepository.GetByUserId(userId) ??
                throw new InvalidOperationException($"RefreshToken with UserId {userId} not found");

            var result = await _unitOfWork.RefreshTokenRepository.RevokeAll(userId);

            _unitOfWork.Commit();

            var tokens = result as RefreshToken[] ?? result.ToArray();
            _logger.Information("Token revoked successfully, {Token}", JsonConvert.SerializeObject(tokens));

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Data = _mapper.Map<IEnumerable<RefreshTokenDto>>(tokens),
                Message = "Token revoked successfully",
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

            if (ex.StackTrace != null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<RefreshTokenDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<RefreshTokenDto>> RevokeToken(string token)
    {
        try
        {
            var principal = _loginService.ValidateToken(token);
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ??
                              throw new InvalidOperationException("Invalid token");

            var isUserIdValid = int.TryParse(userIdClaim, out var userId);

            if (!isUserIdValid) throw new InvalidOperationException("Invalid token");

            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(userId, token) ??
                               throw new InvalidOperationException($"RefreshToken {token} with UserId {userId} not found");

            var result = await _unitOfWork.RefreshTokenRepository.Revoke(refreshToken.RefreshTokenId);

            _unitOfWork.Commit();

            _logger.Information("Token revoked successfully, {Token}", JsonConvert.SerializeObject(result));

            return new ServiceResponse<RefreshTokenDto>
            {
                Data = _mapper.Map<RefreshTokenDto>(result),
                Message = "Token revoked successfully",
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

            if (ex.StackTrace != null) errorMessages.Add(ex.StackTrace);

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