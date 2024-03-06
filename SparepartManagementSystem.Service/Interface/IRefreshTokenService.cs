using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IRefreshTokenService : IService<RefreshTokenDto>
{
    Task<ServiceResponse<RefreshTokenDto>> Revoke(RefreshTokenDto dto);
    Task<ServiceResponse<RefreshTokenDto>> RevokeAll(int userId);
}