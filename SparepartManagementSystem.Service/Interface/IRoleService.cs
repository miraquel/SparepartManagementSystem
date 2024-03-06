﻿using SparepartManagementSystem.Service.DTO;

namespace SparepartManagementSystem.Service.Interface;

public interface IRoleService : IService<RoleDto>
{
    Task<ServiceResponse<RoleDto>> AddUser(UserRoleDto dto);
    Task<ServiceResponse<RoleDto>> DeleteUser(UserRoleDto dto);
    Task<ServiceResponse<IEnumerable<RoleDto>>> GetAllWithUsers();
    Task<ServiceResponse<RoleDto>> GetByIdWithUsers(int id);
}