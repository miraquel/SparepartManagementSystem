using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

internal class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger = Log.ForContext<UserService>();
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoginService _loginService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUnitOfWork unitOfWork, MapperlyMapper mapper, IConfiguration configuration, ILoginService loginService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _loginService = loginService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ServiceResponse> AddUser(UserDto dto)
    {
        try
        {
            if (dto.UserWarehouses is null || dto.UserWarehouses.Count == 0)
            {
                throw new InvalidOperationException("UserWarehouse is required");
            }
            
            var userAdd = _mapper.MapToUser(dto);
            userAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            userAdd.CreatedDateTime = DateTime.Now;
            userAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            userAdd.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.UserRepository.Add(userAdd);
            
            var userId = await _unitOfWork.GetLastInsertedId();

            var userWarehousesAdd = _mapper.MapToListOfUserWarehouse(dto.UserWarehouses).ToArray();
            
            foreach (var userWarehouseAdd in userWarehousesAdd)
            {
                userWarehouseAdd.UserId = userId;
                userWarehouseAdd.CreatedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
                userWarehouseAdd.CreatedDateTime = DateTime.Now;
                userWarehouseAdd.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
                userWarehouseAdd.ModifiedDateTime = DateTime.Now;
                await _unitOfWork.UserWarehouseRepository.Add(userWarehouseAdd);
            }

            _logger.Information("User {UserId} added successfully with UserWarehouse {UserWarehouseId}", userAdd.UserId, string.Join(", ", userWarehousesAdd.Select(uw => uw.UserWarehouseId)));
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "User added successfully with UserWarehouse",
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

    public async Task<ServiceResponse> DeleteUser(int id)
    {
        try
        {
            await _unitOfWork.UserRepository.Delete(id);

            _logger.Information("User {UserId} deleted successfully", id);
            
            _unitOfWork.Commit();

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

    public async Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUser()
    {
        try
        {
            var users = (await _unitOfWork.UserRepository.GetAll()).ToList();

            _logger.Information("Users retrieved successfully with total {TotalRecord} rows", users.Count);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.MapToListOfUserDto(users),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            _logger.Information("User {UserId} retrieved successfully", user.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.MapToUserDto(user),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<UserDto>>> GetUserByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetByParams(parameters);
            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.MapToListOfUserDto(users),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> UpdateUser(UserDto dto)
    {
        try
        {
            var record = await _unitOfWork.UserRepository.GetById(dto.UserId, true);

            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                throw new Exception("User has been modified by another user, please refresh and try again.");
            }

            record.UpdateProperties(_mapper.MapToUser(dto));

            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in User"
                };
            }
            
            record.ModifiedBy = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "";
            record.ModifiedDateTime = DateTime.Now;
            await _unitOfWork.UserRepository.Update(record);

            _logger.Information("User {UserId} updated successfully", dto.UserId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true,
                Message = "User updated successfully"
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

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

    public async Task<ServiceResponse> AddRoleToUser(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.AddUser(dto.RoleId, dto.UserId);

            _unitOfWork.Commit();

            return new ServiceResponse
            {
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> DeleteRoleFromUser(UserRoleDto dto)
    {
        try
        {
            await _unitOfWork.RoleRepository.DeleteUser(dto.RoleId, dto.UserId);

            _unitOfWork.Commit();

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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            return new ServiceResponse
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
                Data = _mapper.MapToListOfUserDto(result),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<UserDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetUserByIdWithRoles(int id)
    {
        try
        {
            var result = await _unitOfWork.UserRepository.GetByIdWithRoles(id);

            _logger.Information("User {UserId} retrieved successfully", result.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.MapToUserDto(result),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetUserByUsernameWithRoles(string username)
    {
        try
        {
            var result = await _unitOfWork.UserRepository.GetByUsernameWithRoles(username);

            _logger.Information("User {Username} retrieved successfully", result.Username);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.MapToUserDto(result),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<UserDto>> GetUserByIdWithUserWarehouse(int id)
    {
        try
        {
            var result = await _unitOfWork.UserRepository.GetByIdWithUserWarehouse(id);

            _logger.Information("User {UserId} retrieved successfully", result.UserId);

            return new ServiceResponse<UserDto>
            {
                Data = _mapper.MapToUserDto(result),
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<UserDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public ServiceResponse<IEnumerable<ActiveDirectoryDto>> GetUsersFromActiveDirectory(string searchText)
    {
        try
        {
            var ldapConnectionString = _configuration.GetSection("ActiveDirectory:ConnectionString").Value;
            var ldapNames = _configuration.GetSection("ActiveDirectory:Names").Value;
            var ldapUsername = _configuration.GetSection("ActiveDirectory:Username").Value;
            var ldapPassword = _configuration.GetSection("ActiveDirectory:Password").Value;

            if (ldapConnectionString == null)
            {
                throw new InvalidOperationException("LDAP Connection String is not found in configuration");
            }

            using var ldapConnection = new LdapConnection(ldapConnectionString);

            ldapConnection.AuthType = AuthType.Basic;
            ldapConnection.Credential = new NetworkCredential(ldapUsername, ldapPassword);

            if (ldapNames == null)
            {
                throw new InvalidOperationException("LDAP Names is not found in configuration");
            }

            string[] attributes = ["givenName", "sn", "sAMAccountName", "userPrincipalName"];

            var ldapFilterMap = new Dictionary<string, string>
            {
                { "givenName", string.IsNullOrEmpty(searchText) ? "*" : $"*{searchText}*" },
                { "sn", string.IsNullOrEmpty(searchText) ? "*" : $"*{searchText}*" },
                { "sAMAccountName", string.IsNullOrEmpty(searchText) ? "*" : $"*{searchText}*" },
                { "userPrincipalName", string.IsNullOrEmpty(searchText) ? "*" : $"*{searchText}*" }
            };

            var ldapFilter = $"(|{string.Join("", ldapFilterMap.Select(x => $"({x.Key}={x.Value})"))})";

            var searchResponse = (SearchResponse)ldapConnection.SendRequest(
                new SearchRequest(
                    ldapNames,
                    ldapFilter,
                    SearchScope.Subtree, attributes));

            var users = from SearchResultEntry entry in searchResponse.Entries
                let firstName = entry.Attributes["givenName"] != null ? entry.Attributes["givenName"][0].ToString() : ""
                let lastName = entry.Attributes["sn"] != null ? entry.Attributes["sn"][0].ToString() : ""
                let username = entry.Attributes["sAMAccountName"] != null ? entry.Attributes["sAMAccountName"][0].ToString() : ""
                let email = entry.Attributes["userPrincipalName"] != null ? entry.Attributes["userPrincipalName"][0].ToString() : ""
                select new ActiveDirectoryDto { FirstName = firstName, LastName = lastName, Username = username, Email = email };

            return new ServiceResponse<IEnumerable<ActiveDirectoryDto>>
            {
                Data = users,
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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            return new ServiceResponse<IEnumerable<ActiveDirectoryDto>>
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
            {
                throw new InvalidOperationException("Login failed");
            }

            var refreshTokenResult = _loginService.GenerateToken(user, true);

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshTokenResult,
                Expires = DateTime.UtcNow.AddSeconds(int.Parse(
                    _configuration.GetSection("Jwt:RefreshTokenExpiration").Value ??
                    throw new InvalidOperationException(
                        "Jwt Configuration not set properly, missing Refresh Token Expiration"))),
                Created = DateTime.UtcNow
            };
            await _unitOfWork.RefreshTokenRepository.Add(newRefreshTokenEntity);

            var token = new TokenDto
            {
                AccessToken = _loginService.GenerateToken(user),
                RefreshToken = refreshTokenResult
            };

            _logger.Information("User {Username} logged in successfully, {User}", user.Username, JsonConvert.SerializeObject(user));
            
            _unitOfWork.Commit();

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

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

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

            var userId = principal.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ?? throw new InvalidOperationException("Invalid token");
            var user = await _unitOfWork.UserRepository.GetByIdWithRoles(int.Parse(userId)) ?? throw new InvalidOperationException($"UserId {userId} not found");
            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(user.UserId, token, true) ?? throw new InvalidOperationException($"RefreshToken {token} with UserId {userId} not found");

            if (!refreshToken.IsActive)
            {
                throw new InvalidOperationException("Invalid token");
            }

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
            
            _unitOfWork.Commit();

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

            if (ex.StackTrace != null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<TokenDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> RevokeAllTokens(int userId)
    {
        try
        {
            _ = _unitOfWork.RefreshTokenRepository.GetByUserId(userId) ??
                throw new InvalidOperationException($"RefreshToken with UserId {userId} not found");

            await _unitOfWork.RefreshTokenRepository.RevokeAll(userId);
            
            _logger.Information("All tokens revoked successfully for UserId {UserId}", userId);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
                Message = "All Tokens revoked successfully",
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

            if (ex.StackTrace != null)
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

    public async Task<ServiceResponse> RevokeToken(string token)
    {
        try
        {
            var principal = _loginService.ValidateToken(token);
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == "userid")?.Value ??
                              throw new InvalidOperationException("Invalid token");

            var isUserIdValid = int.TryParse(userIdClaim, out var userId);

            if (!isUserIdValid)
            {
                throw new InvalidOperationException("Invalid token");
            }

            var refreshToken = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(userId, token) ??
                               throw new InvalidOperationException($"RefreshToken {token} with UserId {userId} not found");

            await _unitOfWork.RefreshTokenRepository.Revoke(refreshToken.RefreshTokenId);

            _logger.Information("Token revoked successfully, {Token}", token);
            
            _unitOfWork.Commit();

            return new ServiceResponse
            {
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

            if (ex.StackTrace != null)
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