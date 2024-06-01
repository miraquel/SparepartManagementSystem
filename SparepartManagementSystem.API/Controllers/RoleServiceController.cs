using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
public class RoleServiceController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleServiceController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllRole()
    {
        var result = await _roleService.GetAllRole();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleDto>> GetRoleById(int id)
    {
        var result = await _roleService.GetRoleById(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoleByParams([FromQuery] Dictionary<string, string> parameters)
    {
        var result = await _roleService.GetRoleByParams(parameters);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Create }])]
    [HttpPost]
    public async Task<ActionResult<RoleDto>> AddRole([FromBody] RoleDto dto)
    {
        var result = await _roleService.AddRole(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Update }])]
    [HttpPut]
    public async Task<ActionResult<RoleDto>> UpdateRole([FromBody] RoleDto dto)
    {
        var result = await _roleService.UpdateRole(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Delete }])]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RoleDto>> DeleteRole(int id)
    {
        var result = await _roleService.DeleteRole(id);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Create }])]
    [HttpPost]
    public async Task<ActionResult<RoleDto>> AddUserToRole([FromBody] UserRoleDto dto)
    {
        var result = await _roleService.AddUserToRole(dto);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Delete }])]
    [HttpDelete]
    public async Task<ActionResult<RoleDto>> DeleteUserFromRole([FromQuery] UserRoleDto dto)
    {
        var result = await _roleService.DeleteUserFromRole(dto);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Read }])]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllWithUsers()
    {
        var result = await _roleService.GetAllRoleWithUsers();
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [MapToApiVersion(1.0)]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = [new[] { PermissionType.RoleActivity.Read }])]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleDto>> GetRoleByIdWithUsers(int id)
    {
        var result = await _roleService.GetRoleByIdWithUsers(id);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}